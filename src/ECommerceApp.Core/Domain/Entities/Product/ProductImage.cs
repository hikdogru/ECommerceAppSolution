using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.Domain.Entities.Product;

/// <summary>
/// Represents an image of a product.
/// </summary>
public class ProductImage
{
    public ProductImage()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    /// <summary>
    /// Gets or sets the unique identifier of the product image.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the path of the image file.
    /// </summary>
    [BsonElement("Path")]
    public string Path { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this image is the default image for the product.
    /// </summary>
    [BsonElement("IsDefault")]
    public bool IsDefault { get; set; }
}
