namespace ECommerceApp.Core.Domain.Entities.Product;

/// <summary>
/// Represents a specification for a product.
/// </summary>
public class Specification : DocumentLongTrack
{
    /// <summary>
    /// Gets or sets the list of specification languages.
    /// </summary>
    public List<SpecificationLanguage> SpecificationLanguages { get; set; }
}
