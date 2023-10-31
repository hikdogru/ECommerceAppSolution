using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.WebUI.Areas.Admin.Models.Catalog;
using ECommerceApp.WebUI.Areas.Admin.Models.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NToastNotify;

namespace ECommerceApp.WebUI.Areas.Admin.Controllers
{

    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        #region Fields

        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;
        private readonly ILanguageService _languageService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly ITagService _tagService;
        private readonly ISpecificationService _specificationService;
        private readonly ISpecificationValueService _specificationValueService;




        #endregion


        #region Ctor

        public ProductController(
             ILogger<ProductController> logger,
             IProductService productService,
             IMapper mapper,
             IToastNotification toastNotification,
             ILanguageService languageService,
             ICategoryService categoryService,
             IBrandService brandService,
             ITagService tagService,
             ISpecificationService specificationService,
             ISpecificationValueService specificationValueService)
        {
            _logger = logger;
            _productService = productService;
            _mapper = mapper;
            _toastNotification = toastNotification;
            _languageService = languageService;
            _categoryService = categoryService;
            _brandService = brandService;
            _tagService = tagService;
            _specificationService = specificationService;
            _specificationValueService = specificationValueService;
        }

        #endregion


        #region Methods

        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Get all products based on filter, page and pageSize.
        /// </summary>
        /// <param name="page">Number of page</param>
        /// <param name="pageSize">Number of page size</param>
        /// <param name="filter">Optional filters</param>
        /// <returns>List of filtered products</returns>
        [HttpPost]
        public async Task<IActionResult> GetAll(int page, int pageSize = 10, GridFilters? filter = null)
        {
            var pagedProducts = await _productService.Filter(page, pageSize, filter);
            var listOfProducts = pagedProducts.Select(x => x);
            var mappedProducts = _mapper.Map<IEnumerable<ProductViewModel>>(listOfProducts).ToList();
            int total = pagedProducts.TotalItems;
            return Json(new { data = mappedProducts, total });
        }

        public IActionResult Create()
        {
            var languages = _languageService.GetAllIfActive();
            var productModel = new ProductModel
            {
                ProductLanguages = languages.Select(l => new ProductLanguageModel
                {
                    LanguageId = l.Id.ToString(),
                    LanguageCode = l.Code
                }).ToList()
            };
            Prepare();
            return View(productModel);
        }


        [HttpPost]
        public IActionResult Create(ProductModel productModel)
        {
            return View(productModel);
        }


        private void Prepare()
        {
            var currentLanguageCode = _languageService.GetCurrentLanguage();
            ViewBag.Categories = _categoryService.GetCategoryTree(currentLanguageCode)
            .Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Id.ToString()
            });

            ViewBag.Tags = _tagService.GetAll()
            .Where(x => !x.IsDeleted)
            .Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            ViewBag.Brands = _brandService.GetAll()
            .Where(x => !x.IsDeleted)
            .Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });


            var currentLanguageId = _languageService.GetCurrentLanguageId();

            ViewBag.Specifications = _specificationService.GetAll()
            .Where(x => !x.IsDeleted &&
            x.SpecificationLanguages.Any(sl => !string.IsNullOrEmpty(sl.Name)
            && sl.LanguageId == currentLanguageId.ToString()))
            .AsEnumerable()
            .Select(x => new SelectListItem
            {
                Text = x.SpecificationLanguages.FirstOrDefault(sl => sl.LanguageId == currentLanguageId.ToString())?.Name,
                Value = x.Id.ToString()
            });
            

            ViewBag.SpecificationValues = _specificationValueService.GetAll()
            .Where(x => !x.IsDeleted &&
            x.SpecificationValueLanguages.Any(sl => !string.IsNullOrEmpty(sl.Value)
            && sl.LanguageId == currentLanguageId.ToString()))
            .AsEnumerable()
            .Select(x => new SelectListItem
            {
                Text = x.SpecificationValueLanguages.FirstOrDefault(sl => sl.LanguageId == currentLanguageId.ToString())?.Value,
                Value = x.Id.ToString()
            });


        }

        #endregion
    }
}