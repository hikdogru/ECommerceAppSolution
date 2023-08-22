using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ECommerceApp.Core.Domain.Entities;

public class CategoryMedia
{
    /// <summary>
    /// Media Id
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    // Language Id
    public string LanguageId { get; set; }

    // Language Code
    public string LanguageCode { get; set; }

    /// <summary>
    /// Media Type (Video, image etc MediaTypes enum)
    /// </summary>
    public int MediaType { get; set; }

    /// <summary>
    /// Size Type (Thumbnail, desktop, mobile etc SizeTypes enum)
    /// </summary>
    public int SizeType { get; set; }

    /// <summary>
    /// Media Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Media path
    /// </summary>
    public string Path { get; set; }
}