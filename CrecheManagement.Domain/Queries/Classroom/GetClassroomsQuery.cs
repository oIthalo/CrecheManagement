using CrecheManagement.Domain.Responses.Classroom;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Queries.Classroom;

public class GetClassroomsQuery : IRequest<BaseResponse<List<ClassroomResponse>>>
{
    public string CrecheIdentifier { get; set; }
    public int? Year { get; set; }
}