using System.Collections;
using System.Xml.Linq;
using AutoMapper;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.DTOs;
using ECommerceApp.Core.Extensions;
using ECommerceApp.Core.Helpers;
using ECommerceApp.WebUI.Models.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using NToastNotify;


namespace ECommerceApp.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly ILanguageService _languageService;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

        #endregion

        #region Ctor

        public CategoryController(ICategoryService categoryService,
            IWebHostEnvironment env,
            IMapper mapper,
            IToastNotification toastNotification,
            ILanguageService languageService)
        {
            _categoryService = categoryService;
            _env = env;
            _mapper = mapper;
            _toastNotification = toastNotification;
            _languageService = languageService;
        }

        #endregion

        #region Methods

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Retrieves a paged collection of categories with filters applied.
        /// </summary>
        /// <param name="page">The index of the requested page.</param>
        /// <param name="pageSize">The maximum number of items per page. Defaults to 10.</param>
        /// <param name="filter">The optional filter to apply to each category.</param>
        [HttpPost]
        public async Task<IActionResult> GetAll(int page, int pageSize = 10, GridFilters? filter = null)
        {
            try
            {
                var pagedCategories = await _categoryService.Filter(page, pageSize, filter);
                var listOfCategories = pagedCategories.Select(x => x);
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

            var languages = _languageService.GetAll().Where(l => !l.IsDeleted);
            var categoryModel = new CategoryModel()
            {
                CategoryLanguages = languages.Select(l => new CategoryLanguageModel()
                {
                    LanguageCode = l.Code,
                    LanguageId = l.Id.ToString()
                }).ToList(),
                CategoryMedias = languages.Select(l => new CategoryMediaModel()
                {
                    LanguageCode = l.Code,
                    LanguageId = l.Id.ToString()
                }).ToList()
            };

            ViewBag.Categories = categories;

            return View(categoryModel);
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
                .Where(c => c.Id != category.Id)
                .Select(c => new SelectListItem()
                {
                    Text = c.Text,
                    Value = c.Id.ToString()
                });

            ViewBag.Categories = categories;

            var languages = _languageService.GetAll().Where(l => !l.IsDeleted);
            var categoryModel = new CategoryModel()
            {
                CategoryLanguages = languages.Select(l => new CategoryLanguageModel()
                {
                    LanguageCode = l.Code,
                    LanguageId = l.Id.ToString()
                }).ToList(),
                CategoryMedias = languages.Select(l => new CategoryMediaModel()
                {
                    LanguageCode = l.Code,
                    LanguageId = l.Id.ToString()
                }).ToList()
            };
            var languageIsNotExist = categoryModel.CategoryLanguages
                .Where(l => !mappedCategory.CategoryLanguages
                    .Any(cl => cl.LanguageId == l.LanguageId))
                .ToList();
            var mediaIsNotExist = categoryModel.CategoryMedias
                .Where(l => !mappedCategory.CategoryMedias
                    .Any(cl => cl.LanguageId == l.LanguageId))
                .ToList();
            mappedCategory.CategoryLanguages.AddRange(languageIsNotExist);
            mappedCategory.CategoryMedias.AddRange(mediaIsNotExist);

            return View(mappedCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var categoryInDB = await _categoryService.GetById(ObjectId.Parse(model.Id));
                if (categoryInDB is null)
                {
                    return NotFound();
                }
                string wwwrootPath = _env.WebRootPath;
                var categoryMedias = model.CategoryMedias.Where(m => m.File != null || !string.IsNullOrEmpty(m.Path)).ToList();
                if (categoryMedias.Count > 0)
                {
                    foreach (var categoryMedia in categoryMedias)
                    {
                        if (categoryMedia?.File != null)
                            categoryMedia.Path = await FileHelper.SaveFile(categoryMedia.File, wwwrootPath);
                    }
                }

                var category = _mapper.Map(model, categoryInDB);
                await _categoryService.Update(category);
                _toastNotification.AddSuccessToastMessage("Category updated successfully!");
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpDelete]
        public async Task Delete(string id)
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


        #endregion

    }

}
