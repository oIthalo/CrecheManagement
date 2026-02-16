using System.Text.Json.Serialization;
using CrecheManagement.Domain.Dtos;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Commands.Attendance;

public class RegisterAttendanceCommand : IRequest<BaseResponse>
{
    [JsonIgnore]
    public string? CrecheIdentifier { get; set; }
    [JsonIgnore]
    public string? ClassroomIdentifier { get; set; }
    public string RegisteredBy { get; set; }
    public DateTime Date { get; set; }
    public List<StudentAttendanceDto> Attendances { get; set; }
}