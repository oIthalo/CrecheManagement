using CrecheManagement.Domain.Responses.Attendance;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Queries.Attendance;

public class GetClassroomAttendancesQuery : IRequest<BaseResponse<List<AttendanceResponse>>>
{
    public string CrecheIdentifier { get; set; }
    public string ClassroomIdentifier { get; set; }
    public DateTime? Date { get; set; }
}