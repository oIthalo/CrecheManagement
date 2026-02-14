namespace CrecheManagement.Domain.Responses.Class;

public record CreatedClassromResponse
{
    public string Name { get; init; }
    public int Year { get; init; }
}