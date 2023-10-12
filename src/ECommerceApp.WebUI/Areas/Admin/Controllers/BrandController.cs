using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.Helpers;
using ECommerceApp.WebUI.Areas.Admin.Models.Catalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using NToastNotify;

namespace ECommerceApp.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        #region Fields
        private readonly ILogger<BrandController> _logger;
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _env;



        #endregion

        #region Ctor

        public BrandController(
            ILogger<BrandController> logger,
            IBrandService brandService,
            IMapper mapper,
            IToastNotification toastNotification,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _brandService = brandService;
            _mapper = mapper;
            _toastNotification = toastNotification;
            _env = env;
        }

        #endregion


        #region Methods

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get all brands based on filter, page and pageSize.
        /// </summary>
        /// <param name="page">Number of page</param>
        /// <param name="pageSize">Number of page size</param>
        /// <param name="filter">Optional filters</param>
        /// <returns>List of filtered languages</returns>
        [HttpPost]
        public async Task<IActionResult> GetAll(int page, int pageSize = 10, GridFilters? filter = null)
        {
            var pagedBrands = await _brandService.Filter(page, pageSize, filter);
            var listOfBrands = pagedBrands.Select(x => x);
            var mappedLanguages = _mapper.Map<IEnumerable<BrandViewModel>>(listOfBrands).ToList();
            int total = pagedBrands.TotalItems;
            return Json(new { data = mappedLanguages, total });
        }


        /// <summary>
        /// Creates a new Brand.
        /// </summary>
        /// <param name="model">The model to create a brand from.</param>
        /// <returns>A JSON result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(BrandModel model)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _env.WebRootPath;
                if (model.File is not null)
                {
                    model.LogoUrl = await FileHelper.SaveFile(model.File, wwwrootPath);
                }

                var brand = _mapper.Map<Brand>(model);
                await _brandService.Insert(brand);
                _toastNotification.AddSuccessToastMessage("Brand created successfully");
                return Json(new { success = true });
            }

            var errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            return Json(new { success = false, errors });
        }

        /// <summary>
        /// Displays the edit view for a brand with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the brand to edit.</param>    
        public async Task<IActionResult> Edit(ObjectId id)
        {
            var brand = await _brandService.GetById(id);
            if (brand is null)
            {
                _toastNotification.AddErrorToastMessage("Brand is not found");
                return Json(new { success = false });
            }

            var mappedBrand = _mapper.Map<BrandModel>(brand);
            return Json(new { success = true, data = mappedBrand });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BrandModel model)
        {
            if (ModelState.IsValid)
            {
                var brandInDB = await _brandService.GetById(ObjectId.Parse(model.Id));
                if (brandInDB is null)
                {
                    return NotFound();
                }

                if (model.File is not null)
                {
                    string wwwrootPath = _env.WebRootPath;
                    model.LogoUrl = await FileHelper.SaveFile(model.File, wwwrootPath);
                }


                var brand = _mapper.Map(model, brandInDB);
                await _brandService.Update(brand);
                _toastNotification.AddSuccessToastMessage("Brand updated successfully");
                return Json(new { success = true });
            }
            var errors = ModelState.ToDictionary(
                           kvp => kvp.Key,
                                      kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                                  );
            return Json(new { success = false, errors });
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            var brand = await _brandService.GetById(id);
            if (brand is null)
            {
                _toastNotification.AddErrorToastMessage("Brand is not found");
                return Json(new { success = false });
            }
            await _brandService.Delete(id);
            _toastNotification.AddSuccessToastMessage("Brand deleted successfully");
            return Json(new { success = true });
        }
        #endregion


    }
}