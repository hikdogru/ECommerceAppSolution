namespace ECommerceApp.WebUI.Areas.Admin.Models.Catalog;
public class VariantSpecificationModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the product specification language.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the variant specification.
    /// </summary>
    public string SpecificationId { get; set; }

    /// <summary>
    /// Gets or sets the value of the variant specification.
    /// </summary>
    public string SpecificationValueId { get; set; }
}
