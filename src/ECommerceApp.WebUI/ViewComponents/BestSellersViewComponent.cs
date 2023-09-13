using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.WebUI.ViewComponents;

public class BestSellersViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}