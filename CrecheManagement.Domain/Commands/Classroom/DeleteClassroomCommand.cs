using MediatR;

namespace CrecheManagement.Domain.Commands.Classroom;

public class DeleteClassroomCommand : IRequest
{
    public string CrecheIdentifier { get; set; }
    public string ClassroomIdentifier { get; set; }
}