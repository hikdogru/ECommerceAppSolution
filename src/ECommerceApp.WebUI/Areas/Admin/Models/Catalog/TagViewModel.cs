using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.WebUI.Areas.Admin.Models.Catalog;

public class TagViewModel
{
    /// <summary>
    /// Represent the Id of tag
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the tag.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the tag.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Is tag Active
    /// </summary>
    public bool IsActive { get; set; }
}
