namespace ECommerceApp.WebUI.Areas.Admin.Models.Catalog;
public class ProductImageModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the product image.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the path of the image file.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this image is the default image for the product.
    /// </summary>
    public bool IsDefault { get; set; }

     public IFormFile? File { get; set; }
}
