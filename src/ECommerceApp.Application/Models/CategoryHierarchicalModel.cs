using ECommerceApp.Core.Domain.Entities.Product;

namespace ECommerceApp.Application.Models;

public class CategoryHierarchicalModel
{
    public CategoryHierarchicalModel(Category category)
    {
        Category = category;
        SubCategories = new List<CategoryHierarchicalModel>();
    }

    public Category Category { get; set; }

    public List<CategoryHierarchicalModel> SubCategories { get; set; }
}