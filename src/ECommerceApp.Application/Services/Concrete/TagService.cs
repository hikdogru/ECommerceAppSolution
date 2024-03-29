using AutoMapper.QueryableExtensions;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Interfaces.Repository;
using ECommerceApp.Core.DTOs;
using ECommerceApp.Core.Extensions;
using ECommerceApp.Infrastructure.Mappings;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Text.RegularExpressions;
using ECommerceApp.Core.Domain.Entities.Product;

namespace ECommerceApp.Application.Services.Concrete;

public class TagService : CRUDService<IRepository<Tag, ObjectId>, Tag, ObjectId>, ITagService
{
    public TagService(IRepository<Tag, ObjectId> repository) : base(repository)
    {
    }

    public async Task<PaginatedList<TagDTO>> Filter(int page, int pageSize, GridFilters? filter = null)
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

        var pagedData = await allData.ProjectTo<TagDTO>(MapperConfig.GetMapper().ConfigurationProvider)
            .ToPagedListAsync(page, pageSize, typeof(IMongoQueryable));

        return pagedData;
    }
}
