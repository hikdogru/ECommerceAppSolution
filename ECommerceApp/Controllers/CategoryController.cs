using AutoMapper;
using ECommerceApp.Core.Domain.Entities;
using ECommerceApp.Core.Helpers;
using ECommerceApp.Core.Services.Abstract;
using ECommerceApp.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using NToastNotify;

namespace ECommerceApp.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

        public CategoryController(ICategoryService categoryService,
            IWebHostEnvironment env,
            IMapper mapper,
            IToastNotification toastNotification)
        {
            _categoryService = categoryService;
            _env = env;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

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
                string wwwrootPath = _env.WebRootPath;
                var categoryMedias = model.CategoryMedias.Where(m => m.File != null).ToList();
                if (categoryMedias.Any())
                {
                    foreach (var categoryMedia in categoryMedias)
                    {
                        categoryMedia.Path = await FileHelper.SaveFile(categoryMedia.File, wwwrootPath);
                    }
                }

                var category = _mapper.Map<Category>(model);
                await _categoryService.Insert(category);
                _toastNotification.AddSuccessToastMessage("Category created successfully!");
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
