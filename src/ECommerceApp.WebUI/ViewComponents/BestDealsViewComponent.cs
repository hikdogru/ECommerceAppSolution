using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.WebUI.ViewComponents;

public class BestDealsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}