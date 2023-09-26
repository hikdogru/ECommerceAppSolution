using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.WebUI.Areas.Admin.Controllers;

public class DictionaryController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}