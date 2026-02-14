using System.Text.Json.Serialization;
using CrecheManagement.Domain.Responses.Class;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Commands.Class;

public class CreateClassroomCommand : IRequest<BaseResponse<CreatedClassromResponse>>
{
    [JsonIgnore]
    public string? CrecheIdentifier { get; set; }
    public string Name { get; set; }
}