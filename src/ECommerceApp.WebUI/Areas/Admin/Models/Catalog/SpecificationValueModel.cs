namespace ECommerceApp.WebUI.Areas.Admin.Models.Catalog;

public class SpecificationValueModel
{
    /// <summary>
    /// Gets or sets the ID of the specification value.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the specification.
    /// </summary>
    public string SpecificationId { get; set; }

    /// <summary>
    /// Gets or sets the list of specification value language models.
    /// </summary>
    public List<SpecificationValueLanguageModel> SpecificationValueLanguages { get; set; }
}
