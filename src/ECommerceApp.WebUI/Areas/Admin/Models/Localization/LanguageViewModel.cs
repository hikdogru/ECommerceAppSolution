using MongoDB.Bson;

namespace ECommerceApp.WebUI.Areas.Admin.Models.Localization;


public class LanguageViewModel
{
    public string Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

