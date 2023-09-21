using ECommerceApp.Application.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.WebUI.ViewComponents;

public class ProductFilterViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;

    public ProductFilterViewComponent(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public IViewComponentResult Invoke()
    {
        var parentCategories = _categoryService.GetHierarchicals();
        return View(parentCategories);
    }
}