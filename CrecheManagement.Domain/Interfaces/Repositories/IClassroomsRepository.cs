using CrecheManagement.Domain.Models;

namespace CrecheManagement.Domain.Interfaces.Repositories;

public interface IClassroomsRepository
{
    Task<List<Classroom>> GetClassroomsAsync(string crecheIdentifier, int year);
    Task<Classroom?> GetByIdentifierAsync(string? classroomIdentifier);
    Task<bool> ExistAsync(string crecheIdentifier, string name, int year);
    Task UpsertAsync(Classroom classroom);
    Task DeleteAsync(Classroom classroom);
}