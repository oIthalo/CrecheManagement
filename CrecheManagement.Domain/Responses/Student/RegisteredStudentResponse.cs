namespace CrecheManagement.Domain.Responses.Student;

public record RegisteredStudentResponse
{
    public string Identifier { get; init; }
    public string RegistrationId { get; init; }
    public string Name { get; init; }
}