using AutoMapper.QueryableExtensions;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.Language;
using ECommerceApp.Core.Domain.Interfaces.Repository;
using ECommerceApp.Core.DTOs;
using ECommerceApp.Core.Extensions;
using ECommerceApp.Infrastructure.Mappings;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Text.RegularExpressions;
using ECommerceApp.Core.Services.Abstract;

namespace ECommerceApp.Application.Services.Concrete;

public class LanguageService : CRUDService<IRepository<Language, ObjectId>, Language, ObjectId>, ILanguageService
{
    private readonly ICookieService _cookieService;
    public LanguageService(
        IRepository<Language, ObjectId> repository,
        ICookieService cookieService) : base(repository)
    {
        _cookieService = cookieService;
    }

    public async Task<PaginatedList<LanguageDTO>> Filter(int page, int pageSize, GridFilters? filter = null)
    {
        var allData = GetAll().Where(x => !x.IsDeleted);

        if (filter != null && filter.Filters != null && filter.Filters.Any() && !string.IsNullOrEmpty(filter.Logic))
        {
            foreach (var field in filter.Filters)
            {
                if (field.Field == "name")
                    allData = allData.Where(x => Regex.IsMatch(x.Name, field.Value, RegexOptions.IgnoreCase));

                else if (field.Field == "isActive")
                    allData = allData.Where(x => x.IsActive == bool.Parse(field.Value));
            }
        }

        var pagedData = await allData.ProjectTo<LanguageDTO>(MapperConfig.GetMapper().ConfigurationProvider)
            .ToPagedListAsync(page, pageSize, typeof(IMongoQueryable));

        return pagedData;
    }

    public IQueryable<LanguageDTO> GetAllIfActive()
    {
        return GetAllIfIsNotDeleted().Where(x => x.IsActive);
    }

    public IQueryable<LanguageDTO> GetAllIfIsNotDeleted()
    {
        var languages = GetAll().Where(x => !x.IsDeleted);
        return languages.ProjectTo<LanguageDTO>(MapperConfig.GetMapper().ConfigurationProvider);
    }

    public LanguageDTO GetByCode(string languageCode)
    {
        return GetAllIfActive().FirstOrDefault(l => l.Code == languageCode);
    }

    public string GetCurrentLanguage()
    {
        string langugeCookieName = "EcommerceApp_UserLanguage";
        var cookieLanguageValue = _cookieService.GetCookieValue(langugeCookieName);
        if (cookieLanguageValue is not null)
        {
            return cookieLanguageValue;
        }

        // Todo: Change this code after completed the parameter module
        return "tr";
    }

    public ObjectId GetCurrentLanguageId()
    {
        string currentLanguageCode = GetCurrentLanguage();
        var currentLanguage = GetByCode(currentLanguageCode);
        if (currentLanguage is LanguageDTO)
        {
            return currentLanguage.Id;
        }

        return ObjectId.Empty;
    }
}