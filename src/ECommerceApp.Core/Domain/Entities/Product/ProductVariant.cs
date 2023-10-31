using ECommerceApp.Core.Domain.Entities.Language;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.Domain.Entities.Product;
/// <summary>
/// Represents a variant of a product.
/// </summary>
public class ProductVariant
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductVariant"/> class.
    /// </summary>
    public ProductVariant()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    /// <summary>
    /// Gets or sets the identifier of the product variant.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the barcode of the product variant.
    /// </summary>
    [BsonElement("Barcode")]
    public string Barcode { get; set; }

    /// <summary>
    /// Gets or sets the stock code of the product variant.
    /// </summary>
    [BsonElement("StockCode")]
    public string StockCode { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product variant.
    /// </summary>
    [BsonElement("Quantity")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Gets or sets the specifications of the product variant.
    /// </summary>
    [BsonElement("Specifications")]
    public List<ProductVariantSpecification> Specifications { get; set; }

}
