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

public class SpecificationService : CRUDService<IRepository<Specification, ObjectId>, Specification, ObjectId>, ISpecificationService
{
    private readonly ILanguageService _languageService;
    public SpecificationService(IRepository<Specification, ObjectId> repository, ILanguageService languageService) : base(repository)
    {
        _languageService = languageService;
    }

    public async Task<PaginatedList<SpecificationDTO>> Filter(int page, int pageSize, GridFilters? filter = null)
    {
        var currentLanguageId = _languageService.GetCurrentLanguageId();
        var allData = GetAll().Where(x => !x.IsDeleted && 
        x.SpecificationLanguages.Any(cl => !string.IsNullOrEmpty(cl.Name) && Regex.IsMatch(cl.LanguageId, currentLanguageId.ToString(), RegexOptions.IgnoreCase)));

        if (filter != null && filter.Filters != null && filter.Filters.Any() && !string.IsNullOrEmpty(filter.Logic))
        {
            foreach (var field in filter.Filters)
            {
                if (field.Field == "name")
                    allData = allData.Where(x => x.SpecificationLanguages.Any(cl => Regex.IsMatch(cl.Name, field.Value, RegexOptions.IgnoreCase)));
            }
        }

        var pagedData = await allData.ProjectTo<SpecificationDTO>(MapperConfig.GetMapper().ConfigurationProvider)
            .ToPagedListAsync(page, pageSize, typeof(IMongoQueryable));

        return pagedData;
    }
}
