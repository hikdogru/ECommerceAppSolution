using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.WebUI.Areas.Admin.Models.Localization;

public class DictionaryModel
{
    public string Id { get; set; }

    // Represents dictionary key e.g UI.Home
    public string Key { get; set; } 

    // Represents dictionary value e.g Home
    public string Value { get; set; } 

    // Represents dictionary language id
    public string LanguageId { get; set; }
}
