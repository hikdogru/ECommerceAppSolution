using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.Domain.Entities.Language;

/// <summary>
/// Represents a product specification.
/// </summary>
public class ProductSpecification
{
    // <summary>
    /// Initializes a new instance of the <see cref="ProductSpecification"/> class.
    /// </summary>
     public ProductSpecification()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    /// <summary>
    /// Gets or sets the unique identifier for the product specification.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }


    /// <summary>
    /// Gets or sets the name of the product specification.
    /// </summary>
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("SpecificationId")]
    public string SpecificationId { get; set; }

    /// <summary>
    /// Gets or sets the value of the product specification.
    /// </summary>
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("SpecificationValueId")]
    public string SpecificationValueId { get; set; }
}
