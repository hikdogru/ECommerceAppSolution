using AutoMapper;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.Language;
using ECommerceApp.WebUI.Areas.Admin.Models.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using NToastNotify;

namespace ECommerceApp.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class DictionaryController : Controller
{
    #region Fields


    private readonly IDictionaryService _dictionaryService;
    private readonly IMapper _mapper;
    private readonly IToastNotification _toastNotification;
    private readonly ILanguageService _languageService;

    #endregion

    #region Ctor

    public DictionaryController(
        IDictionaryService dictionaryService,
        IMapper mapper,
        IToastNotification toastNotification
,
        ILanguageService languageService)
    {
        _dictionaryService = dictionaryService;
        _mapper = mapper;
        _toastNotification = toastNotification;
        _languageService = languageService;
    }

    #endregion

    #region Methods


    // GET
    public IActionResult Index()
    {
        ViewBag.Languages = _languageService.GetAllIfIsNotDeleted().Select(l => new SelectListItem
        {
            Text = l.Name,
            Value = l.Id.ToString()
        });
        return View();
    }

    /// <summary>
    /// Get all dictionaries based on filter, page and pageSize.
    /// </summary>
    /// <param name="page">Number of page</param>
    /// <param name="pageSize">Number of page size</param>
    /// <param name="filter">Optional filters</param>
    /// <returns>List of filtered dictionaries</returns>
    [HttpPost]
    public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10, GridFilters? filter = null)
    {
        var pageddictionaries = await _dictionaryService.Filter(page, pageSize, filter);
        var listOfdictionaries = pageddictionaries.Select(x => x);
        var mappeddictionaries = _mapper.Map<IEnumerable<DictionaryViewModel>>(listOfdictionaries)
                                        .ToList();
        mappeddictionaries.ForEach(d => d.Language =  _languageService.GetById(ObjectId.Parse(d.LanguageId)).Result.Name);
        int total = pageddictionaries.TotalItems;
        return Json(new { data = mappeddictionaries, total });
    }


    /// <summary>
    /// Creates a new dictionary.
    /// </summary>
    /// <param name="model">The model to create a dictionary from.</param>
    /// <returns>A JSON result.</returns>
    [HttpPost]
    public async Task<IActionResult> Create(DictionaryModel model)
    {
        if (ModelState.IsValid)
        {
            var dictionary = _mapper.Map<Dictionary>(model);
            await _dictionaryService.Insert(dictionary);
            _toastNotification.AddSuccessToastMessage("Dictionary created successfully");
            return Json(new { success = true });
        }

        var errors = ModelState.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
        );

        return Json(new { success = false, errors });
    }

    /// <summary>
    /// Displays the edit view for a dictionary with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the dictionary to edit.</param>    
    public async Task<IActionResult> Edit(ObjectId id)
    {
        var dictionary = await _dictionaryService.GetById(id);
        // Control if dictionary is null
        if (dictionary is null)
        {
            _toastNotification.AddErrorToastMessage("Dictionary is not found");
            return Json(new { success = false });
        }

        var mappedDictionary = _mapper.Map<DictionaryModel>(dictionary);
        return Json(new { success = true, data = mappedDictionary });
    }


    [HttpPost]
    public async Task<IActionResult> Edit(DictionaryModel model)
    {
        if (ModelState.IsValid)
        {
            var dictionaryInDB = await _dictionaryService.GetById(ObjectId.Parse(model.Id));
            if (dictionaryInDB is null)
            {
                return NotFound();
            }
            var dictionary = _mapper.Map(model, dictionaryInDB);
            await _dictionaryService.Update(dictionary);
            _toastNotification.AddSuccessToastMessage("Dictionary updated successfully!");
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
        var dictionary = await _dictionaryService.GetById(id);
        if (dictionary is null)
        {
            _toastNotification.AddErrorToastMessage("Dictionary is not found");
            return Json(new { success = false });
        }
        await _dictionaryService.Delete(id);
        _toastNotification.AddSuccessToastMessage("Dictionary deleted successfully!");
        return Json(new { success = true });
    }

    #endregion
}