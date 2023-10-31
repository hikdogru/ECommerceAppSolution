namespace ECommerceApp.WebUI.Areas.Admin.Models.Catalog;

/// <summary>
/// Represents a specification model.
/// </summary>
public class SpecificationModel
{
    /// <summary>
    /// Gets or sets the ID of the specification.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the list of specification language models.
    /// </summary>
    public List<SpecificationLanguageModel> SpecificationLanguages { get; set; }
}
