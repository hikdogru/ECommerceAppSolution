using System.Collections;
using AutoMapper;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities;
using ECommerceApp.Core.DTOs;
using ECommerceApp.Core.Extensions;
using ECommerceApp.Core.Helpers;
using ECommerceApp.WebUI.Models.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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

        [HttpPost]
        public async Task<IActionResult> GetAll(int page, int pageSize = 10, GridFilters? filter = null)
        {
            try
            {
                // Todo : Filtrelemeye devam edilecek. 
                var pagedCategories = await _categoryService.Filter(page, pageSize, filter);
                var listOfCategories = pagedCategories.Select((x) => x);
                var mappedCategories = _mapper.Map<IEnumerable<CategoryViewModel>>(listOfCategories);
                int total = pagedCategories.TotalItems;
                return Json(new { data = mappedCategories, total });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
