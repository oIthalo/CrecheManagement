using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CrecheManagement.Domain.Models;

public abstract class BaseModel
{
    [BsonId]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    public string Identifier { get; set; }
    public DateTime CreatedAt { get; set; }

    public BaseModel()
    {
        Id = ObjectId.GenerateNewId();
        Identifier = Guid.NewGuid().ToString("N");
        CreatedAt = DateTime.Now;
    }
}