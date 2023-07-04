using ECommerceApp.Core.Helpers;
using ECommerceApp.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Create()
        {
            return View(new CategoryModel()
            {
                CategoryLanguages = new List<CategoryLanguageModel>()
                {
                    new ()
                    {
                        LanguageCode = "English",
                        LanguageId = "1"
                    },
                    new ()
                    {
                        LanguageCode = "Turkish",
                        LanguageId = "2"
                    }
                },
                CategoryMedias = new List<CategoryMediaModel>()
                {
                    new ()
                    {
                        LanguageId = "1",
                        LanguageCode = "English"
                    },
                    new ()
                    {
                        LanguageId = "2",
                        LanguageCode = "Turkish"
                    }
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var categoryLanguage in model.CategoryLanguages)
                {

                }
                await FileHelper.SaveFile(model.CategoryMedias[0].File);

            }
            return View(model);
        }
    }
}
