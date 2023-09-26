using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ECommerceApp.Core.Domain.Entities.Language;

/// <summary>
/// Represents a dictionary entity
/// </summary>
public class Dictionary : DocumentLongTrack
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    // Represents dictionary key e.g UI.Home
    [BsonElement("Key")]
    public string Key { get; set; }

    // Represents dictionary value e.g Home
    [BsonElement("Value")]
    public string Value { get; set; }

    // Represents dictionary language id
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("LanguageId")]
    public string LanguageId { get; set; }

    [BsonIgnore]
    public Language Language { get; set; }
}