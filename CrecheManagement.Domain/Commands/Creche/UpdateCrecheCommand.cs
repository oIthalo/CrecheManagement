using System.Text.Json.Serialization;
using CrecheManagement.Domain.ValueObjects;
using MediatR;

namespace CrecheManagement.Domain.Commands.Creche;

public class UpdateCrecheCommand : IRequest
{
    [JsonIgnore]
    public string? Identifier { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? ContactNumber { get; set; }
    public Address? Address { get; set; }
}