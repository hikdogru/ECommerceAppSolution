using System.Collections;
using System.Xml.Linq;
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
using Microsoft.AspNetCore.Mvc.Rendering;
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
                var pagedCategories = await _categoryService.Filter(page, pageSize, filter);
                var listOfCategories = pagedCategories.Select((x) => x);
                var mappedCategories = _mapper.Map<IEnumerable<CategoryViewModel>>(listOfCategories).ToList();
                var categoryHierarchies = _categoryService.GetCategoryTree().Where(c => mappedCategories.Any(mc => mc.Id == c.Id));
                mappedCategories.ForEach(c => c.Hierarchy = categoryHierarchies.FirstOrDefault(ch => ch.Id == c.Id)?.Text);
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
            var categories = _categoryService.GetCategoryTree()
                                            .ToList()
                                            .Select(c => new SelectListItem()
                                            {
                                                Text = c.Text,
                                                Value = c.Id.ToString()
                                            });

            ViewBag.Categories = categories;

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

        [HttpGet]
        public async Task<IActionResult> Edit(ObjectId id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                _toastNotification.AddErrorToastMessage("Id is null or empty!");
            }

            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            var mappedCategory = _mapper.Map<CategoryModel>(category);
            var categories = _categoryService.GetCategoryTree()
                .ToList()
                .Select(c => new SelectListItem()
                {
                    Text = c.Text,
                    Value = c.Id.ToString()
                });

            ViewBag.Categories = categories;
            return View(mappedCategory);
        }

        [HttpDelete]
        public async Task Delete(string id, object result)
        {
            if (string.IsNullOrEmpty(id))
            {
                _toastNotification.AddErrorToastMessage("Id is null or empty!");
            }
            var category = _categoryService.GetAll().ToList().FirstOrDefault(x => x.Id == ObjectId.Parse(id));
            if (category == null)
            {
                _toastNotification.AddErrorToastMessage("Category not found!");
            }
            await _categoryService.Delete(category);
            _toastNotification.AddSuccessToastMessage("Category deleted successfully!");
        }


    }


}
