using CrecheManagement.Domain.Enums;

namespace CrecheManagement.Domain.Dtos;

public record StudentAttendanceDto
{
    public string StudentIdentifier { get; init; }
    public EAttendanceStatus Status { get; init; }
    public string? Justification { get; init; }
}