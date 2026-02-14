using CrecheManagement.Domain.Responses.Student;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Queries.Student;

public class GetStudentsQuery : IRequest<BaseResponse<List<StudentResponse>>>
{
    public string CrecheIdentifier { get; set; }
    public string? ClassroomIdentifier { get; set; }
}