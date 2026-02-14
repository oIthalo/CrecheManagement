namespace CrecheManagement.Domain.Dtos;

public record ClassroomStudentsDto
{
    public string Identifier { get; init; }
    public string RegistrationId { get; init; }
    public string Name { get; init; }
}