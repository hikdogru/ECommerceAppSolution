using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.WebUI.Controllers;

public class ProductController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult WishList()
    {
        return View();
    }

    public IActionResult Compare()
    {
        return View();
    }

    public IActionResult Details()
    {
        return View();
    }
}