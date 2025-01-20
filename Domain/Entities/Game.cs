using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class Game
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid MySqlId { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
    [BsonElement("price")]
    public double Price { get; set; }

    public IEnumerable<Category>? Categories { get; set; } = [];

    [BsonIgnore]
    public ICollection<GameCategory>? GameCategories { get; set; }
}