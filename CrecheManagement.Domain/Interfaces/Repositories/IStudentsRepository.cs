using CrecheManagement.Domain.Models;

namespace CrecheManagement.Domain.Interfaces.Repositories;

public interface IStudentsRepository
{
    Task<List<Student>> GetStudentsByCrecheAsync(string crecheIdentifier);
    Task<List<Student>> GetStudentsByClassroomAsync(string classroomIdentifier);
    Task<List<Student>> GetStudentsAsync(List<string> studentsIdentifier);
    Task<Student?> GetStudentAsync(string studentIdentifier);
    Task<bool> ExistStudentWithCPFAsync(string crecheIdentifier, string cpf);
    Task UpsertAsync(Student student);
    Task UpsertRangeAsync(List<Student> students);
}