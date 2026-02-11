using CrecheManagement.Domain.ValueObjects;

namespace CrecheManagement.Domain.Responses.Creche;

public class CrecheResponse
{
    public string Identifier { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string ContactNumber { get; set; }
    public Address Address { get; set; }
}