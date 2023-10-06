using System.Diagnostics;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Services.Abstract;
using ECommerceApp.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICookieService _cookieService;
        private readonly IDictionaryService _dictionaryService;
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger,
        ICookieService cookieService,
        IDictionaryService languageService)
        {
            _logger = logger;
            _cookieService = cookieService;
            _dictionaryService = languageService;
        }

        public async Task<IActionResult> Index()
        {        
            return View();
        }

        public IActionResult ChangeUILanguage(string languageCode)
        {
            if(!string.IsNullOrEmpty(languageCode))
            {
                _cookieService.SetCookieValue("EcommerceApp_UserLanguage", languageCode);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {
            if (statusCode.HasValue && statusCode == StatusCodes.Status404NotFound)
            {
                return View("404");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
