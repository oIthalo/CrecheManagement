using CrecheManagement.Domain.ValueObjects;

namespace CrecheManagement.Domain.Responses.Creche;

public record CrecheResponse
{
    public string Identifier { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public string ContactNumber { get; init; }
    public Address Address { get; init; }
}