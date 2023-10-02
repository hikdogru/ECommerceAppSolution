using AutoMapper.QueryableExtensions;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.Language;
using ECommerceApp.Core.Domain.Interfaces.Repository;
using ECommerceApp.Core.DTOs;
using ECommerceApp.Core.Extensions;
using ECommerceApp.Infrastructure.Mappings;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ECommerceApp.Application.Services.Concrete;

public class DictionaryService : CRUDService<IRepository<Dictionary, ObjectId>, Dictionary, ObjectId>, IDictionaryService
{
    private readonly ILanguageService _languageService;
    public DictionaryService(
        IRepository<Dictionary, ObjectId> repository,
        ILanguageService languageService
        ) : base(repository)
    {
        _languageService = languageService;
    }

    public async Task<PaginatedList<DictionaryDTO>> Filter(int page, int pageSize, GridFilters? filter = null)
    {
        var allData = GetAll().Where(x => !x.IsDeleted);

        if (filter != null && filter.Filters != null && filter.Filters.Any() && !string.IsNullOrEmpty(filter.Logic))
        {

            var objectIdList = new List<ObjectId>();
            foreach (var field in filter.Filters)
            {
                if (field.Field == "key" || field.Field == "value")
                    allData = allData.Where(x => Regex.IsMatch(x.Key, field.Value, RegexOptions.IgnoreCase) ||
                                                 Regex.IsMatch(x.Value, field.Value, RegexOptions.IgnoreCase));
                if (field.Field == "language")
                {
                    var language = _languageService.GetAllIfIsNotDeleted()
                                                     .FirstOrDefault(l => l.Name.Equals(field.Value));
                    if (language is LanguageDTO)
                    {
                        var dictionaryIds = allData.Where(x => x.LanguageId == language.Id.ToString())
                                                  .Select(x => x.Id);
                        objectIdList.AddRange(dictionaryIds);
                    }
                }
            }
            if(objectIdList.Count > 0)
                allData = allData.Where(x => objectIdList.Contains(x.Id));

        }

        var pagedData = await allData.ProjectTo<DictionaryDTO>(MapperConfig.GetMapper().ConfigurationProvider)
            .ToPagedListAsync(page, pageSize, typeof(IMongoQueryable));

        return pagedData;
    }
}