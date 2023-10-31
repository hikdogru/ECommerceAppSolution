namespace ECommerceApp.WebUI.Areas.Admin.Models.Catalog;

public class ProductModel
{
    /// <summary>
    /// Gets or sets the ID of the product.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The code of the product.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// The group code of the product.
    /// </summary>
    public string GroupCode { get; set; }

    /// <summary>
    /// The price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The Tax rate of product
    /// </summary>  
    public int TaxRate { get; set; }

    /// <summary>
    /// The total stock quantity of the product.
    /// </summary>
    public decimal TotalQuantity { get; set; }

    public bool IsItOffSale { get; set; }

    /// <summary>
    /// The unique identifier for the brand of the product.
    /// </summary>
    public string BrandId { get; set; }

    /// <summary>
    /// The list of unique identifiers for the categories that the product belongs to.
    /// </summary>
    public List<string> CategoryIds { get; set; }

    /// <summary>
    /// The list of unique identifiers for the tags that are associated with the product.
    /// </summary>
    public List<string> TagIds { get; set; }

    /// <summary>
    /// The list of images of the product.
    /// </summary>
    public List<ProductImageModel> Images { get; set; }

    /// <summary>
    /// Product Languages
    /// </summary>
    public List<ProductLanguageModel> ProductLanguages { get; set; }

    /// <summary>
    /// Product Variants
    /// </summary>
    public List<ProductVariantModel> ProductVariants { get; set; }

    public List<ProductSpecificationModel> ProductSpecifications { get; set; }
}
