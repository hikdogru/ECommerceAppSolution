using AutoMapper.QueryableExtensions;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.Domain.Interfaces.Repository;
using ECommerceApp.Core.DTOs;
using ECommerceApp.Core.Extensions;
using ECommerceApp.Infrastructure.Mappings;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Text.RegularExpressions;

namespace ECommerceApp.Application.Services.Concrete;
public class SpecificationValueService : CRUDService<IRepository<SpecificationValue, ObjectId>, SpecificationValue, ObjectId>, ISpecificationValueService
{
    private readonly ILanguageService _languageService;
    public SpecificationValueService(IRepository<SpecificationValue, ObjectId> repository,
    ILanguageService languageService) : base(repository)
    {
        _languageService = languageService;
    }

    public async Task<PaginatedList<SpecificationValueDTO>> Filter(int page, int pageSize, GridFilters? filter = null)
    {
        var currentLanguageId = _languageService.GetCurrentLanguageId();
        var allData = GetAll().Where(x => !x.IsDeleted &&
        x.SpecificationValueLanguages.Any(cl => !string.IsNullOrEmpty(cl.Value) && Regex.IsMatch(cl.LanguageId, currentLanguageId.ToString(), RegexOptions.IgnoreCase)));

        if (filter != null && filter.Filters != null && filter.Filters.Any() && !string.IsNullOrEmpty(filter.Logic))
        {
            foreach (var field in filter.Filters)
            {
                if (field.Field == "value")
                    allData = allData.Where(x => x.SpecificationValueLanguages.Any(cl => Regex.IsMatch(cl.Value, field.Value, RegexOptions.IgnoreCase)));
            }
        }

        var pagedData = await allData.ProjectTo<SpecificationValueDTO>(MapperConfig.GetMapper().ConfigurationProvider)
            .ToPagedListAsync(page, pageSize, typeof(IMongoQueryable));

        return pagedData;
    }
}
