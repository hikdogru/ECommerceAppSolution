using MongoDB.Bson;

namespace ECommerceApp.WebUI.Models.Category;

public class CategoryViewModel
{
    public ObjectId Id { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }

    public string? MediaPath { get; set; }

    public string? Hierarchy { get; set; }
}