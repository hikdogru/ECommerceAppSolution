using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.WebUI.Areas.Admin.Models.Localization
{
    public class DictionaryViewModel
    {
        public string Id { get; set; } = string.Empty;

        // Represents dictionary key e.g UI.Home
        public string Key { get; set; } = string.Empty;

        // Represents dictionary value e.g Home
        public string Value { get; set; } = string.Empty;

        // Represents language name e.g English
        public string Language { get; set; } = string.Empty;
        public string LanguageId { get; set; } = string.Empty;


    }
}