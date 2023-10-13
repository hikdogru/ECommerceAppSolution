using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.WebUI.Areas.Admin.Models.System;

public class ParameterViewModel
{
    /// <summary>
    /// Represent the Id of parameter
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the parameter
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the parameter
    /// </summary>
    public string Value { get; set; } = string.Empty;
}
