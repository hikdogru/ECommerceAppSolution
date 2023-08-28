using Microsoft.AspNetCore.Mvc;

namespace WebPanel.Admin.ViewComponents
{
    public class SideMenuViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {

            return View();
        }
    }
}
