using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.Domain.Entities.Product;

public class ProductLanguage
{
    public ProductLanguage()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    /// <summary>
    /// Product Language Id
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    // Language Id
    public string LanguageId { get; set; }

    // Language Code
    public string LanguageCode { get; set; }

    // Product Name
    public string Name { get; set; }

    // Product Description
    public string Description { get; set; }
}
