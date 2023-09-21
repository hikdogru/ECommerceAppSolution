using MongoDB.Bson;

namespace ECommerceApp.WebUI.Areas.Admin.Models.Localization;

public class LanguageModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Direction { get; set; }
    public bool IsActive { get; set; }
}