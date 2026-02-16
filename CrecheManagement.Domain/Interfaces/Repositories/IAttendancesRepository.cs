using CrecheManagement.Domain.Models;

namespace CrecheManagement.Domain.Interfaces.Repositories;

public interface IAttendancesRepository
{
    Task<List<Attendance>> GetClassroomAttendancesAsync(string classroomIdentifier, DateTime? date = null);
    Task<List<string>> GetStudentsWithAttendanceRegisteredToday(string classroomIdentifier);
    Task InsertRangeAsync(List<Attendance> attendances);
}