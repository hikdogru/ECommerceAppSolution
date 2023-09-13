using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.WebUI.ViewComponents;

public class UIBannerViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}