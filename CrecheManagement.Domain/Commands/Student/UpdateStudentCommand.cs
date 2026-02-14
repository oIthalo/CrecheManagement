using System.Text.Json.Serialization;
using MediatR;

namespace CrecheManagement.Domain.Commands.Student;

public class UpdateStudentCommand : IRequest
{
    [JsonIgnore]
    public string? CrecheIdentifier { get; set; }
    [JsonIgnore]
    public string? StudentIdentifier { get; set; }
    public string? ClassroomIdentifier { get; set; }
    public string? ContactNumber { get; set; }
    public bool? Active { get; set; }
}