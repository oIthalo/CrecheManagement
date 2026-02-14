using System.Text.Json.Serialization;
using CrecheManagement.Domain.Dtos;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Commands.Classroom;

public class InsertStudentsToClassroomCommand : IRequest<BaseResponse<List<ClassroomStudentsDto>>>
{
    [JsonIgnore]
    public string? CrecheIdentifier { get; set; }

    [JsonIgnore]
    public string? ClassroomIdentifier { get; set; }

    public List<string> StudentsIdentifiers { get; set; }
}