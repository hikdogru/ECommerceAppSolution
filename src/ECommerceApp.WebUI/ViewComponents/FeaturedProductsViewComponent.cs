using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.WebUI.ViewComponents;

public class FeaturedProductsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}