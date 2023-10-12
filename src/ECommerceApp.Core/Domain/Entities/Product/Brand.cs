using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.Domain.Entities.Product;

/// <summary>
/// Represents a brand of a product.
/// </summary>
public class Brand : DocumentLongTrack
{
    /// <summary>
    /// Gets or sets the name of the brand.
    /// </summary>
    [BsonElement("Name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the brand.
    /// </summary>
    [BsonElement("Description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the URL of the brand's logo.
    /// </summary>
    [BsonElement("LogoUrl")]
    public string LogoUrl { get; set; }

    /// <summary>
    /// Is Brand Active
    /// </summary>
    [BsonElement("IsActive")]
    public bool IsActive { get; set; }
}
