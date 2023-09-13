using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.WebUI.ViewComponents;

public class SingleProductViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}