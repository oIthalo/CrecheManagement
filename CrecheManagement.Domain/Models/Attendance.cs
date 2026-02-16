using CrecheManagement.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CrecheManagement.Domain.Models;

public class Attendance : BaseModel
{
    public string CrecheIdentifier { get; set; }
    public string ClassroomIdentifier { get; set; }
    public string StudentIdentifier { get; set; }
    public string StudentName { get; set; }
    public string RegisteredBy { get; set; }
    public DateTime Date { get; set; }
    [BsonRepresentation(BsonType.String)]
    public EAttendanceStatus Status { get; set; }
    public string? Justification { get; set; }
}