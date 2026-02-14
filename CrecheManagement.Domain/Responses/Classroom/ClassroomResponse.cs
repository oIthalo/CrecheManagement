using CrecheManagement.Domain.Dtos;

namespace CrecheManagement.Domain.Responses.Classroom;

public class ClassroomResponse
{
    public string Identifier { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public List<ClassroomStudentsDto> Students { get; set; }
}