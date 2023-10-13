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
    public class TagController : Controller
    {

        #region Fields

        private readonly ILogger<TagController> _logger;
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;


        #endregion

        #region Ctor

        public TagController(
            ILogger<TagController> logger,
            ITagService tagService,
            IMapper mapper,
            IToastNotification toastNotification)
        {
            _logger = logger;
            _tagService = tagService;
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
        /// Get all tags based on filter, page and pageSize.
        /// </summary>
        /// <param name="page">Number of page</param>
        /// <param name="pageSize">Number of page size</param>
        /// <param name="filter">Optional filters</param>
        /// <returns>List of filtered tags</returns>
        [HttpPost]
        public async Task<IActionResult> GetAll(int page, int pageSize = 10, GridFilters? filter = null)
        {
            var pagedTags = await _tagService.Filter(page, pageSize, filter);
            var listOfTags = pagedTags.Select(x => x);
            var mappedTags = _mapper.Map<IEnumerable<TagViewModel>>(listOfTags).ToList();
            int total = pagedTags.TotalItems;
            return Json(new { data = mappedTags, total });
        }

        /// <summary>
        /// Creates a new Tag.
        /// </summary>
        /// <param name="model">The model to create a tag from.</param>
        /// <returns>A JSON result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(TagModel model)
        {
            if (ModelState.IsValid)
            {
                var tag = _mapper.Map<Tag>(model);
                await _tagService.Insert(tag);
                _toastNotification.AddSuccessToastMessage("Tag created successfully");
                return Json(new { success = true });
            }

            var errors = GetModelStateErrors(ModelState);
            return Json(new { success = false, errors });
        }


        /// <summary>
        /// Displays the edit view for a tag with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the tag to edit.</param>    
        public async Task<IActionResult> Edit(ObjectId id)
        {
            var tag = await _tagService.GetById(id);
            if (tag is null)
            {
                _toastNotification.AddErrorToastMessage("Tag is not found");
                return Json(new { success = false });
            }

            var mappedTag = _mapper.Map<TagModel>(tag);
            return Json(new { success = true, data = mappedTag });
        }


        /// <summary>
        /// Handles the HTTP POST request to update a tag with the specified model data.
        /// </summary>
        /// <param name="model">The model data representing the tag to update.</param>
        /// <returns>An asynchronous <see cref="Task"/> representing the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(TagModel model)
        {
            if (ModelState.IsValid)
            {
                var tagInDB = await _tagService.GetById(ObjectId.Parse(model.Id));
                if (tagInDB is null)
                {
                    return NotFound();
                }

                var tag = _mapper.Map(model, tagInDB);
                await _tagService.Update(tag);
                _toastNotification.AddSuccessToastMessage("Tag updated successfully");
                return Json(new { success = true });
            }
            var errors = GetModelStateErrors(ModelState);
            return Json(new { success = false, errors });
        }

        private Dictionary<string, string[]?> GetModelStateErrors(ModelStateDictionary modelState)
        {
            var errors = modelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            return errors;
        }


        /// <summary>
        /// Deletes a tag with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the tag to delete.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            var tag = await _tagService.GetById(id);
            if (tag is null)
            {
                _toastNotification.AddErrorToastMessage("Tag is not found");
                return Json(new { success = false });
            }
            await _tagService.Delete(id);
            _toastNotification.AddSuccessToastMessage("Tag deleted successfully");
            return Json(new { success = true });
        }
        
        #endregion

    }
}