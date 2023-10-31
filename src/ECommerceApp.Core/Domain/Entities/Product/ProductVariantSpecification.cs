using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.Domain.Entities.Language;

/// <summary>
/// Represents a language-specific variant specification.
/// </summary>
public class ProductVariantSpecification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductVariantSpecification"/> class.
    /// </summary>
    public ProductVariantSpecification()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    /// <summary>
    /// Gets or sets the unique identifier for the variant specification.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the variant specification.
    /// </summary>
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("SpecificationId")]
    public string SpecificationId { get; set; }

    /// <summary>
    /// Gets or sets the value of the variant specification.
    /// </summary>
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("SpecificationValueId")]
    public string SpecificationValueId { get; set; }
}
