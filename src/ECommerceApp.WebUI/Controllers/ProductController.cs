using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.WebUI.Controllers;

public class ProductController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}