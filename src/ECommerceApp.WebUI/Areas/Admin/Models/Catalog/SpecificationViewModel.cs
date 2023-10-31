namespace ECommerceApp.WebUI.Areas.Admin.Models.Catalog;

/// <summary>
/// Represents a view model for a specification.
/// </summary>
public class SpecificationViewModel
{
    /// <summary>
    /// Gets or sets the ID of the specification.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the specification.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the language of the specification.
    /// </summary>
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the language of the specification.
    /// </summary>
    public string LanguageId { get; set; } = string.Empty;
}
