using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.Domain.Entities.Product;

public class SpecificationLanguage
{
    public SpecificationLanguage()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    /// <summary>
    /// Specification Language Id
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    // Language Id
    public string LanguageId { get; set; }

    // Language Code
    public string LanguageCode { get; set; }

    // Specification Name
    public string Name { get; set; }

    // Specification Description
    public string Description { get; set; }
}
