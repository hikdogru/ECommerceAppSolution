namespace ECommerceApp.WebUI.Areas.Admin.Models.Catalog;
public class ProductVariantModel
{
    /// <summary>
    /// Gets or sets the identifier of the product variant.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the barcode of the product variant.
    /// </summary>
    public string Barcode { get; set; }

    /// <summary>
    /// Gets or sets the stock code of the product variant.
    /// </summary>
    public string StockCode { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product variant.
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Gets or sets the specifications of the product variant.
    /// </summary>
    public List<VariantSpecificationModel> Specifications { get; set; }
}
