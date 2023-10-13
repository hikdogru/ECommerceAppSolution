using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.System;
using ECommerceApp.WebUI.Areas.Admin.Models.System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using NToastNotify;

namespace ECommerceApp.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ParameterController : Controller
    {
        #region Fields

        private readonly ILogger<ParameterController> _logger;
        private readonly IParameterService _parameterService;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;


        #endregion


        #region Ctor

        public ParameterController(
             ILogger<ParameterController> logger,
             IParameterService parameterService,
             IMapper mapper,
             IToastNotification toastNotification)
        {
            _logger = logger;
            _parameterService = parameterService;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        #endregion


        #region Methods

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get all parameters based on filter, page and pageSize.
        /// </summary>
        /// <param name="page">Number of page</param>
        /// <param name="pageSize">Number of page size</param>
        /// <param name="filter">Optional filters</param>
        /// <returns>List of filtered parameters</returns>
        [HttpPost]
        public async Task<IActionResult> GetAll(int page, int pageSize = 10, GridFilters? filter = null)
        {
            var pagedParameters = await _parameterService.Filter(page, pageSize, filter);
            var listOfParameters = pagedParameters.Select(x => x);
            var mappedParameters = _mapper.Map<IEnumerable<ParameterViewModel>>(listOfParameters).ToList();
            int total = pagedParameters.TotalItems;
            return Json(new { data = mappedParameters, total });
        }

        /// <summary>
        /// Creates a new Parameter.
        /// </summary>
        /// <param name="model">The model to create a parameter from.</param>
        /// <returns>A JSON result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(ParameterModel model)
        {
            if (ModelState.IsValid)
            {
                var parameter = _mapper.Map<Parameter>(model);
                await _parameterService.Insert(parameter);
                _toastNotification.AddSuccessToastMessage("Parameter created successfully");
                return Json(new { success = true });
            }

            var errors = GetModelStateErrors(ModelState);
            return Json(new { success = false, errors });
        }


        /// <summary>
        /// Displays the edit view for a parameter with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the parameter to edit.</param>    
        public async Task<IActionResult> Edit(ObjectId id)
        {
            var parameter = await _parameterService.GetById(id);
            if (parameter is null)
            {
                _toastNotification.AddErrorToastMessage("Parameter is not found");
                return Json(new { success = false });
            }

            var mappedParameter = _mapper.Map<ParameterModel>(parameter);
            return Json(new { success = true, data = mappedParameter });
        }


        /// <summary>
        /// Handles the HTTP POST request to update a parameter with the specified model data.
        /// </summary>
        /// <param name="model">The model data representing the parameter to update.</param>
        /// <returns>An asynchronous <see cref="Task"/> representing the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(ParameterModel model)
        {
            if (ModelState.IsValid)
            {
                var parameterInDB = await _parameterService.GetById(ObjectId.Parse(model.Id));
                if (parameterInDB is null)
                {
                    return NotFound();
                }

                var parameter = _mapper.Map(model, parameterInDB);
                await _parameterService.Update(parameter);
                _toastNotification.AddSuccessToastMessage("Parameter updated successfully");
                return Json(new { success = true });
            }
            var errors = GetModelStateErrors(ModelState);
            return Json(new { success = false, errors });
        }



        /// <summary>
        /// Deletes a parameter with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the parameter to delete.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            var parameter = await _parameterService.GetById(id);
            if (parameter is null)
            {
                _toastNotification.AddErrorToastMessage("Parameter is not found");
                return Json(new { success = false });
            }
            await _parameterService.Delete(id);
            _toastNotification.AddSuccessToastMessage("Parameter deleted successfully");
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