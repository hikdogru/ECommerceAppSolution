using ECommerceApp.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ECommerceApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger
            )
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            //var category = new Category
            //{
            //    IsActive = false,
            //    ParentId = ObjectId.GenerateNewId().ToString(),
            //    CategoryLanguages = new List<CategoryLanguage>
            //    {
            //        new()
            //        {
            //            LanguageCode = "en",
            //            LanguageId = Guid.NewGuid().ToString(),
            //            Name = "Test Category",
            //            Description = "Test category description",
            //            SortNr = 10
            //        }
            //    }
            //};

            //await _categoryRepository.Add(category);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}