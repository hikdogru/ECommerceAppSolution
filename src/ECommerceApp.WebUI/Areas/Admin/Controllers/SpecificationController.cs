using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.WebUI.Areas.Admin.Models.Catalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using NToastNotify;

namespace ECommerceApp.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecificationController : Controller
    {
        #region Fields
        private readonly ILogger<SpecificationController> _logger;
        private readonly ISpecificationService _specificationService;
        private readonly IMapper _mapper;
        private readonly ILanguageService _languageService;
        private readonly IToastNotification _toastNotification;

        #endregion


        #region Ctor
        public SpecificationController(
         ILogger<SpecificationController> logger,
         ISpecificationService specificationService,
         IMapper mapper,
         ILanguageService languageService,
         IToastNotification toastNotification)
        {
            _logger = logger;
            _specificationService = specificationService;
            _mapper = mapper;
            _languageService = languageService;
            _toastNotification = toastNotification;
        }

        #endregion


        #region Methods


        public IActionResult Index()
        {
            var model = new SpecificationModel
            {
                SpecificationLanguages = _languageService.GetAllIfActive().Select(l => new SpecificationLanguageModel
                {
                    LanguageId = l.Id.ToString(),
                    LanguageCode = l.Code
                }).ToList()
            };
            return View(model);
        }


        /// <summary>
        /// Get all specifications based on filter, page and pageSize.
        /// </summary>
        /// <param name="page">Number of page</param>
        /// <param name="pageSize">Number of page size</param>
        /// <param name="filter">Optional filters</param>
        /// <returns>List of filtered specifications</returns>
        [HttpPost]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10, GridFilters? filter = null)
        {
            var pagedSpecifications = await _specificationService.Filter(page, pageSize, filter);
            var listOfSpecifications = pagedSpecifications.Select(x => x);
            var mappedSpecifications = _mapper.Map<IEnumerable<SpecificationViewModel>>(listOfSpecifications)
                                            .ToList();

            var currentLanguage = _languageService.GetCurrentLanguageId();
            foreach (var specification in mappedSpecifications)
            {
                specification.LanguageId = listOfSpecifications.Where(x => x.Id == specification.Id)
                .SelectMany(x => x.SpecificationLanguages)
                .FirstOrDefault(x => x.LanguageId == currentLanguage.ToString()).LanguageId;

                specification.Language = _languageService.GetById(currentLanguage).Result.Name;

                specification.Name = listOfSpecifications.Where(x => x.Id == specification.Id)
                .SelectMany(x => x.SpecificationLanguages)
                .FirstOrDefault(x => x.LanguageId == currentLanguage.ToString())?.Name ?? "-";
            }
            int total = pagedSpecifications.TotalItems;
            return Json(new { data = mappedSpecifications, total });
        }


        [HttpPost]
        public async Task<IActionResult> Create(SpecificationModel model)
        {
            if (ModelState.IsValid)
            {
                var specification = _mapper.Map<Specification>(model);
                await _specificationService.Insert(specification);
                _toastNotification.AddSuccessToastMessage("Specification created successfully!");
                return Json(new { success = true });
            }

            var errors = GetModelStateErrors(ModelState);
            return Json(new { success = false, errors });
        }




        /// <summary>
        /// Displays the edit view for a specification with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the specification to edit.</param>    
        public async Task<IActionResult> Edit(ObjectId id)
        {
            var specification = await _specificationService.GetById(id);
            if (specification is null)
            {
                _toastNotification.AddErrorToastMessage("Specification is not found");
                return Json(new { success = false });
            }

            var mappedSpecification = _mapper.Map<SpecificationModel>(specification);
            return Json(new { success = true, data = mappedSpecification });
        }


        /// <summary>
        /// Handles the HTTP POST request to update a specification with the specified model data.
        /// </summary>
        /// <param name="model">The model data representing the specification to update.</param>
        /// <returns>An asynchronous <see cref="Task"/> representing the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(SpecificationModel model)
        {
            if (ModelState.IsValid)
            {
                var specificationInDB = await _specificationService.GetById(ObjectId.Parse(model.Id));
                if (specificationInDB is null)
                {
                    return NotFound();
                }

                var specification = _mapper.Map(model, specificationInDB);
                await _specificationService.Update(specification);
                _toastNotification.AddSuccessToastMessage("Specification updated successfully");
                return Json(new { success = true });
            }
            var errors = GetModelStateErrors(ModelState);
            return Json(new { success = false, errors });
        }


        /// <summary>
        /// Deletes a specification with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the specification to delete.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            var specification = await _specificationService.GetById(id);
            if (specification is null)
            {
                _toastNotification.AddErrorToastMessage("Specification is not found");
                return Json(new { success = false });
            }
            await _specificationService.Delete(id);
            _toastNotification.AddSuccessToastMessage("Specification deleted successfully");
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