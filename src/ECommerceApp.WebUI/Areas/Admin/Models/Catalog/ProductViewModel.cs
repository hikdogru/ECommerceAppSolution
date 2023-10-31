namespace ECommerceApp.WebUI.Areas.Admin.Models.Catalog;

/// <summary>
/// Represents a view model for a product.
/// </summary>
public class ProductViewModel
{
    /// <summary>
    /// Gets or sets the ID of the product.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the code of the product.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the total stock quantity of the product.
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the product is off sale.
    /// </summary>
    public bool IsItOffSale { get; set; }

    /// <summary>
    /// Gets or sets the brand name of the product.
    /// </summary>
    public string BrandName { get; set; }

    /// <summary>
    /// Gets or sets the image of the product.
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string Name { get; set; }
}
