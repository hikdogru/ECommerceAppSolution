using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.WebUI.Controllers;

public class OrderController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult TrackOrder()
    {
        return View();
    }
}