using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.WebUI.ViewComponents;

public class ShopCategoryViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}