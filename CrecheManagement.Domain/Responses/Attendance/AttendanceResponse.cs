namespace CrecheManagement.Domain.Responses.Attendance;

public record AttendanceResponse
{
    public string StudentName { get; init; }
    public string RegisteredBy { get; init; }
    public DateTime Date { get; init; }
    public string Status { get; init; }
    public string? Justification { get; init; }
}