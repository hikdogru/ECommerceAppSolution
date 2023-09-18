using AutoMapper;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.WebUI.Models.Category;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.WebUI.ViewComponents;

public class ShopCategoryViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public ShopCategoryViewComponent(ICategoryService categoryService,
        IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var (page, pageSize) = (1, 10);
        var paginatedCategories = await _categoryService.Filter(page, pageSize);
        var categories = paginatedCategories.Select(x => x);
        var mappedCategories = _mapper.Map<IEnumerable<CategoryViewModel>>(categories).ToList();
        return View(mappedCategories);
    }
}