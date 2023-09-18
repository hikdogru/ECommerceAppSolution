using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.WebUI.Controllers;

public class ShoppingCartController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}