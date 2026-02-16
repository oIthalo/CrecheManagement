using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Models;
using CrecheManagement.Infrastructure.Context;
using MongoDB.Driver;

namespace CrecheManagement.Infrastructure.Repositories;

public class AttendancesRepository : IAttendancesRepository
{
    private readonly MongoContext _mongo;

    public AttendancesRepository(MongoContext mongo)
    {
        _mongo = mongo;
    }

    public async Task<List<Attendance>> GetClassroomAttendancesAsync(string classroomIdentifier, DateTime? date = null)
    {
        var filters = new List<FilterDefinition<Attendance>>
        {
            Builders<Attendance>.Filter.Eq(a => a.ClassroomIdentifier, classroomIdentifier)
        };

        if (date.HasValue)
        {
            var today = date.Value.Date;
            var tomorrow = today.AddDays(1);

            filters.Add(Builders<Attendance>.Filter.Gte(a => a.Date, today));
            filters.Add(Builders<Attendance>.Filter.Lt(a => a.Date, tomorrow));
        }

        var filter = Builders<Attendance>.Filter.And(filters);

        return await _mongo.Attendances.Find(filter).ToListAsync();
    }

    public async Task<List<string>> GetStudentsWithAttendanceRegisteredToday(string classroomIdentifier)
    {
        var today = DateTime.Now.Date;
        var tomorrow = today.AddDays(1);

        var filter = Builders<Attendance>.Filter.And(
            Builders<Attendance>.Filter.Eq(a => a.ClassroomIdentifier, classroomIdentifier),
            Builders<Attendance>.Filter.Gte(a => a.Date, today),
            Builders<Attendance>.Filter.Lt(a => a.Date, tomorrow)
        );

        var attendances = await _mongo.Attendances.Find(filter).ToListAsync();
        return attendances.Select(a => a.StudentIdentifier).ToList();
    }

    public async Task InsertRangeAsync(List<Attendance> attendances)
    {
        var models = new List<WriteModel<Attendance>>();

        foreach (var attendance in attendances)
        {
            var insertOneModel = new InsertOneModel<Attendance>(attendance);
            models.Add(insertOneModel);
        }

        await _mongo.Attendances.BulkWriteAsync(models);
    }
}