using AutoMapper;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.Language;
using ECommerceApp.WebUI.Areas.Admin.Models.Localization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NToastNotify;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerceApp.WebUI.Areas.Admin.Controllers;
[Area("Admin")]
public class LanguageController : Controller
{
    #region Fields

    private readonly ILanguageService _languageService;
    private readonly IMapper _mapper;
    private readonly IToastNotification _toastNotification;


    #endregion

    #region Ctor

    public LanguageController(
        ILanguageService languageService,
        IMapper mapper, IToastNotification toastNotification)
    {
        _languageService = languageService;
        _mapper = mapper;
        _toastNotification = toastNotification;
    }

    #endregion

    #region Methods

    // GET
    public IActionResult Index()
    {
        return View();
    }


    /// <summary>
    /// Get all languages based on filter, page and pageSize.
    /// </summary>
    /// <param name="page">Number of page</param>
    /// <param name="pageSize">Number of page size</param>
    /// <param name="filter">Optional filters</param>
    /// <returns>List of filtered languages</returns>
    [HttpPost]
    public async Task<IActionResult> GetAll(int page, int pageSize = 10, GridFilters? filter = null)
    {
        var pagedLanguages = await _languageService.Filter(page, pageSize, filter);
        var listOfLanguages = pagedLanguages.Select(x => x);
        var mappedLanguages = _mapper.Map<IEnumerable<LanguageViewModel>>(listOfLanguages).ToList();
        int total = pagedLanguages.TotalItems;
        return Json(new { data = mappedLanguages, total });
    }


    /// <summary>
    /// Creates a new Language.
    /// </summary>
    /// <param name="model">The model to create a Language from.</param>
    /// <returns>A JSON result.</returns>
    [HttpPost]
    public async Task<IActionResult> Create(LanguageModel model)
    {
        if (ModelState.IsValid)
        {
            var language = _mapper.Map<Language>(model);
            await _languageService.Insert(language);
            _toastNotification.AddSuccessToastMessage("Language created successfully");
            return Json(new { success = true });
        }

        var errors = ModelState.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
        );

        return Json(new { success = false, errors });
    }

    public async Task<IActionResult> Edit(ObjectId id)
    {
        var language = await _languageService.GetById(id);
        // Control if language is null
        if (language is null)
        {
            _toastNotification.AddErrorToastMessage("Language is not found");
            return Json(new { success = false });
        }

        var mappedLanguage = _mapper.Map<LanguageModel>(language);
        return Json(new { success = true, data = mappedLanguage });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(LanguageModel model)
    {
        if (ModelState.IsValid)
        {
            var languageInDB = await _languageService.GetById(ObjectId.Parse(model.Id));
            if (languageInDB is null)
            {
                return NotFound();
            }
            var language = _mapper.Map(model, languageInDB);
            await _languageService.Update(language);
            _toastNotification.AddSuccessToastMessage("Language updated successfully");
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
        var language = await _languageService.GetById(id);
        if (language is null)
        {
            _toastNotification.AddErrorToastMessage("Language is not found");
            return Json(new { success = false });
        }
        await _languageService.Delete(id);
        _toastNotification.AddSuccessToastMessage("Language deleted successfully");
        return Json(new { success = true });
    }

    #endregion

}