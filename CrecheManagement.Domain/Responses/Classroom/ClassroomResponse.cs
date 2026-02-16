using CrecheManagement.Domain.Dtos;

namespace CrecheManagement.Domain.Responses.Classroom;

public record ClassroomResponse
{
    public string Identifier { get; init; }
    public string Name { get; init; }
    public int Year { get; init; }
    public List<ClassroomStudentsDto> Students { get; init; }
}