using ECommerceApp.Core.Domain.Entities.Language;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.Domain.Entities.Product;


/// <summary>
/// Represents a product in the e-commerce application.
/// </summary>
public class Product : DocumentLongTrack
{

    /// <summary>
    /// The code of the product.
    /// </summary>
    [BsonElement("Code")]
    public string Code { get; set; }

    /// <summary>
    /// The group code of the product.
    /// </summary>
    [BsonElement("GroupCode")]
    public string GroupCode { get; set; }

    /// <summary>
    /// The price of the product.
    /// </summary>
    [BsonElement("Price")]
    public decimal Price { get; set; }

    /// <summary>
    /// The Tax rate of product
    /// </summary>
    [BsonElement("TaxRate")]
    public int TaxRate { get; set; }

    /// <summary>
    /// The total stock quantity of the product.
    /// </summary>
    [BsonElement("TotalQuantity")]
    public decimal TotalQuantity { get; set; }

    [BsonElement("IsItOffSale")]
    public bool IsItOffSale { get; set; }

    /// <summary>
    /// The unique identifier for the brand of the product.
    /// </summary>
    [BsonElement("Brand")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string BrandId { get; set; }

    /// <summary>
    /// The list of unique identifiers for the categories that the product belongs to.
    /// </summary>
    [BsonElement("Categories")]
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> CategoryIds { get; set; }

    /// <summary>
    /// The list of unique identifiers for the tags that are associated with the product.
    /// </summary>
    [BsonElement("Tags")]
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> TagIds { get; set; }

    /// <summary>
    /// The list of images of the product.
    /// </summary>
    [BsonElement("Images")]
    public List<ProductImage> Images { get; set; }

    /// <summary>
    /// Product Languages
    /// </summary>
    public List<ProductLanguage> ProductLanguages { get; set; }

    /// <summary>
    /// Product Variants
    /// </summary>
    public List<ProductVariant> ProductVariants { get; set; }

    public List<ProductSpecification> ProductSpecifications { get; set; }

}
