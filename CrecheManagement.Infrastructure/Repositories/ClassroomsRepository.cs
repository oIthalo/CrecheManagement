using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Models;
using CrecheManagement.Infrastructure.Context;
using MongoDB.Driver;

namespace CrecheManagement.Infrastructure.Repositories;

public class ClassroomsRepository : IClassroomsRepository
{
    private readonly MongoContext _mongo;

    public ClassroomsRepository(MongoContext mongo)
    {
        _mongo = mongo;
    }

    public async Task<List<Classroom>> GetClassroomsAsync(string crecheIdentifier, int year)
    {
        var filter = Builders<Classroom>.Filter.And(
            Builders<Classroom>.Filter.Eq(c => c.CrecheIdentifier, crecheIdentifier),
            Builders<Classroom>.Filter.Eq(c => c.Year, year)
        );
        return await _mongo.Classrooms.Find(filter).ToListAsync();
    }

    public async Task<Classroom?> GetByIdentifierAsync(string? classroomIdentifier)
    {
        var filter = Builders<Classroom>.Filter.Eq(c => c.Identifier, classroomIdentifier);
        return await _mongo.Classrooms.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<bool> ExistAsync(string crecheIdentifier, string name, int year)
    {
        var filter = Builders<Classroom>.Filter.And(
            Builders<Classroom>.Filter.Eq(c => c.CrecheIdentifier, crecheIdentifier),
            Builders<Classroom>.Filter.Eq(c => c.Name, name),
            Builders<Classroom>.Filter.Eq(c => c.Year, year)
        );

        return await _mongo.Classrooms.Find(filter).AnyAsync();
    }

    public async Task UpsertAsync(Classroom classroom)
    {
        var filter = Builders<Classroom>.Filter.Eq(c => c.CrecheIdentifier, classroom.CrecheIdentifier);
        await _mongo.Classrooms.ReplaceOneAsync(filter, classroom, new ReplaceOptions { IsUpsert = true });
    }

    public async Task DeleteAsync(Classroom classroom)
    {
        await _mongo.Classrooms.DeleteOneAsync(c => c.Identifier == classroom.Identifier);
    }
}