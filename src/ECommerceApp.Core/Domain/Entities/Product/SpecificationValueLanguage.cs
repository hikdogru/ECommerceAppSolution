using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.Domain.Entities.Product;

public class SpecificationValueLanguage
{
    public SpecificationValueLanguage()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    /// <summary>
    /// Specification value Language Id
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    // Language Id
    public string LanguageId { get; set; }

    // Language Code
    public string LanguageCode { get; set; }

    // Specification value
    public string Value { get; set; }
}
