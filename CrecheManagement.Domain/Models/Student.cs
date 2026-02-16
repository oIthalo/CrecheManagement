using CrecheManagement.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CrecheManagement.Domain.Models;

public class Student : BaseModel
{
    public string CrecheIdentifier { get; set; }
    public string? ClassroomIdentifier { get; set; }
    public string? Classroom { get; set; }
    public string Name { get; set; }
    public string CPF { get; set; }
    public string? ContactNumber { get; set; }
    public DateTime BirthDate { get; set; }
    [BsonRepresentation(BsonType.String)]
    public EGender Gender { get; set; }
    public string RegistrationId { get; set; }
    public DateTime DateRegistration { get; set; }
    public bool Active { get; set; }
    public List<string> Documents { get; set; }

    public Student()
    {
        Active = true;
        DateRegistration = DateTime.Now.Date;
        Documents = new List<string>();
        RegistrationId = Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper();
    }
}