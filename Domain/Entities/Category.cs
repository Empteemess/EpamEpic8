using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class Category
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid MySqlId { get; set; }
    [BsonElement("name")]
    public string Name { get; set; }
    
    [BsonIgnore] 
    public ICollection<GameCategory> GameCategories { get; set; }
}