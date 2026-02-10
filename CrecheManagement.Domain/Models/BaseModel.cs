using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CrecheManagement.Domain.Models;

public abstract class BaseModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Identifier { get; set; }
    public DateTime CreatedAt { get; set; }

    public BaseModel()
    {
        Identifier = Guid.NewGuid().ToString("N");
        CreatedAt = DateTime.Now;
    }
}