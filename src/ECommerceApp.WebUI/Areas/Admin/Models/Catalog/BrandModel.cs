using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.WebUI.Areas.Admin.Models.Catalog;

public class BrandModel
{
    /// <summary>
    /// Represent the Id of Brand
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the brand.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the brand.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the URL of the brand's logo.
    /// </summary>
    public string LogoUrl { get; set; }

    /// <summary>
    /// Is Brand Active
    /// </summary>
    public bool IsActive { get; set; }

    public IFormFile? File { get; set; }
}
