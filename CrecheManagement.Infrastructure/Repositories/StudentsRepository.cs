using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Models;
using CrecheManagement.Infrastructure.Context;
using MongoDB.Driver;

namespace CrecheManagement.Infrastructure.Repositories;

public class StudentsRepository : IStudentsRepository
{
    private readonly MongoContext _mongo;

    public StudentsRepository(MongoContext mongo)
    {
        _mongo = mongo;
    }

    public async Task<List<Student>> GetStudentsByCrecheAsync(string crecheIdentifier)
    {
        var filter = Builders<Student>.Filter.Eq(s => s.CrecheIdentifier, crecheIdentifier);
        return await _mongo.Students.Find(filter).ToListAsync();
    }

    public async Task<List<Student>> GetStudentsByClassroomAsync(string classroomIdentifier)
    {
        var filter = Builders<Student>.Filter.Eq(s => s.ClassroomIdentifier, classroomIdentifier);
        return await _mongo.Students.Find(filter).ToListAsync();
    }

    public async Task<List<Student>> GetStudentsAsync(List<string> studentsIdentifier)
    {
        var filter = Builders<Student>.Filter.In(s => s.Identifier, studentsIdentifier);
        return await _mongo.Students.Find(filter).ToListAsync();
    }

    public async Task<Student?> GetStudentAsync(string studentIdentifier)
    {
        var filter = Builders<Student>.Filter.Eq(s => s.Identifier, studentIdentifier);
        return await _mongo.Students.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<bool> ExistStudentWithCPFAsync(string crecheIdentifier, string cpf)
    {
        var filter = Builders<Student>.Filter.And(
            Builders<Student>.Filter.Eq(s => s.CPF, cpf),
            Builders<Student>.Filter.Eq(s => s.CrecheIdentifier, crecheIdentifier)
            );

        return await _mongo.Students.Find(filter).AnyAsync();
    }

    public async Task UpsertAsync(Student student)
    {
        var filter = Builders<Student>.Filter.Eq(s => s.CrecheIdentifier, student.CrecheIdentifier);
        await _mongo.Students.ReplaceOneAsync(filter, student, new ReplaceOptions { IsUpsert = true });
    }

    public async Task UpsertRangeAsync(List<Student> students)
    {
        var models = new List<WriteModel<Student>>();

        foreach (var student in students)
        {
            var filter = Builders<Student>.Filter.Eq(s => s.Identifier, student.Identifier);
            var replaceModel = new ReplaceOneModel<Student>(filter, student) { IsUpsert = true };

            models.Add(replaceModel);
        }

        await _mongo.Students.BulkWriteAsync(models);
    }
}