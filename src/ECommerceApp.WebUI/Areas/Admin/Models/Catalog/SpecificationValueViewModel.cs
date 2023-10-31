namespace ECommerceApp.WebUI.Areas.Admin.Models.Catalog;

public class SpecificationValueViewModel
{
    /// <summary>
    /// Gets or sets the ID of the specification value.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the specification.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the specification.
    /// </summary>
    public string SpecificationId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the specification value.
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the language of the specification value.
    /// </summary>
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the language of the specification value.
    /// </summary>
    public string LanguageId { get; set; } = string.Empty;
}
