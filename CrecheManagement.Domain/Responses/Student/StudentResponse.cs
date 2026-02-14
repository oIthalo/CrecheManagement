namespace CrecheManagement.Domain.Responses.Student;

public record StudentResponse
{
    public string Identifier { get; init; }
    public string? Classroom { get; init; }
    public string Name { get; init; }
    public string CPF { get; init; }
    public string? ContactNumber { get; init; }
    public int Age { get; init; }
    public DateTime BirthDate { get; init; }
    public string Gender { get; init; }
    public string RegistrationId { get; init; }
    public DateTime DateRegistration { get; init; }
    public bool Active { get; init; }
    public List<string> Documents { get; init; }
}