using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.WebUI.Areas.Admin.Models.Catalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using NToastNotify;

namespace ECommerceApp.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecificationValueController : Controller
    {
        #region Fields

        private readonly ILogger<SpecificationController> _logger;
        private readonly ISpecificationService _specificationService;
        private readonly ISpecificationValueService _specificationValueService;

        private readonly IMapper _mapper;
        private readonly ILanguageService _languageService;
        private readonly IToastNotification _toastNotification;


        #endregion



        #region Ctor

        public SpecificationValueController(
         ILogger<SpecificationController> logger,
         ISpecificationService specificationService,
         IMapper mapper,
         ILanguageService languageService,
         IToastNotification toastNotification,
         ISpecificationValueService specificationValueService)
        {
            _logger = logger;
            _specificationService = specificationService;
            _mapper = mapper;
            _languageService = languageService;
            _toastNotification = toastNotification;
            _specificationValueService = specificationValueService;
        }

        #endregion


        #region Methods


        public IActionResult Index()
        {
            var model = new SpecificationValueModel
            {
                SpecificationValueLanguages = _languageService.GetAllIfActive().Select(l => new SpecificationValueLanguageModel
                {
                    LanguageId = l.Id.ToString(),
                    LanguageCode = l.Code
                }).ToList()
            };
            var currentLanguageId = _languageService.GetCurrentLanguageId();
            ViewBag.Specifications = _specificationService.GetAll()
            .Where(x => x.SpecificationLanguages.Any(sl => !string.IsNullOrEmpty(sl.Name)
            && sl.LanguageId == currentLanguageId.ToString()))
            .AsEnumerable()
                .Select(x => new SelectListItem()
                {
                    Text = x.SpecificationLanguages.FirstOrDefault(sl => sl.LanguageId == currentLanguageId.ToString()).Name,
                    Value = x.Id.ToString()
                });
            return View(model);
        }


        /// <summary>
        /// Get all specification values based on filter, page and pageSize.
        /// </summary>
        /// <param name="page">Number of page</param>
        /// <param name="pageSize">Number of page size</param>
        /// <param name="filter">Optional filters</param>
        /// <returns>List of filtered specification values</returns>
        [HttpPost]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10, GridFilters? filter = null)
        {
            var pagedSpecificationValues = await _specificationValueService.Filter(page, pageSize, filter);
            var listOfSpecificationValues = pagedSpecificationValues.Select(x => x);
            var mappedSpecificationValues = _mapper.Map<IEnumerable<SpecificationValueViewModel>>(listOfSpecificationValues)
                                            .ToList();

            var currentLanguage = _languageService.GetCurrentLanguageId();
            foreach (var specificationValue in mappedSpecificationValues)
            {
                specificationValue.LanguageId = listOfSpecificationValues.Where(x => x.Id == specificationValue.Id)
                .SelectMany(x => x.SpecificationValueLanguages)
                .FirstOrDefault(x => x.LanguageId == currentLanguage.ToString()).LanguageId;

                specificationValue.Language = _languageService.GetById(currentLanguage).Result.Name;

                specificationValue.Value = listOfSpecificationValues.Where(x => x.Id == specificationValue.Id)
                .SelectMany(x => x.SpecificationValueLanguages)
                .FirstOrDefault(x => x.LanguageId == currentLanguage.ToString())?.Value ?? "-";

                var specification = await _specificationService.GetById(ObjectId.Parse(specificationValue.SpecificationId));
                specificationValue.Name = specification.SpecificationLanguages.FirstOrDefault(x => x.LanguageId == currentLanguage.ToString()).Name;
            }
            int total = pagedSpecificationValues.TotalItems;
            return Json(new { data = mappedSpecificationValues, total });
        }


        [HttpPost]
        public async Task<IActionResult> Create(SpecificationValueModel model)
        {
            if (ModelState.IsValid)
            {
                var specificationValue = _mapper.Map<SpecificationValue>(model);
                await _specificationValueService.Insert(specificationValue);
                _toastNotification.AddSuccessToastMessage("Specification value created successfully!");
                return Json(new { success = true });
            }

            var errors = GetModelStateErrors(ModelState);
            return Json(new { success = false, errors });
        }


        /// <summary>
        /// Displays the edit view for a specification value with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the specification value to edit.</param>    
        public async Task<IActionResult> Edit(ObjectId id)
        {
            var specificationValue = await _specificationValueService.GetById(id);
            if (specificationValue is null)
            {
                _toastNotification.AddErrorToastMessage("Specification value is not found");
                return Json(new { success = false });
            }

            var mappedSpecificationValue = _mapper.Map<SpecificationValueModel>(specificationValue);
            return Json(new { success = true, data = mappedSpecificationValue });
        }


        /// <summary>
        /// Handles the HTTP POST request to update a specification value with the specified model data.
        /// </summary>
        /// <param name="model">The model data representing the specification value to update.</param>
        /// <returns>An asynchronous <see cref="Task"/> representing the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(SpecificationValueModel model)
        {
            if (ModelState.IsValid)
            {
                var specificationValueInDB = await _specificationValueService.GetById(ObjectId.Parse(model.Id));
                if (specificationValueInDB is null)
                {
                    return NotFound();
                }

                var specificationValue = _mapper.Map(model, specificationValueInDB);
                await _specificationValueService.Update(specificationValue);
                _toastNotification.AddSuccessToastMessage("Specification value updated successfully");
                return Json(new { success = true });
            }
            var errors = GetModelStateErrors(ModelState);
            return Json(new { success = false, errors });
        }


        /// <summary>
        /// Deletes a specification value with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the specification value to delete.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            var specificationValue = await _specificationValueService.GetById(id);
            if (specificationValue is null)
            {
                _toastNotification.AddErrorToastMessage("Specification value is not found");
                return Json(new { success = false });
            }
            await _specificationValueService.Delete(id);
            _toastNotification.AddSuccessToastMessage("Specification value deleted successfully");
            return Json(new { success = true });
        }

        private Dictionary<string, string[]?> GetModelStateErrors(ModelStateDictionary modelState)
        {
            var errors = modelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            return errors;
        }

        #endregion
    }
}