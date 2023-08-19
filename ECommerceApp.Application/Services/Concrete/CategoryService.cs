using AutoMapper.QueryableExtensions;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities;
using ECommerceApp.Core.Domain.Interfaces.Repository;
using ECommerceApp.Core.DTOs;
using ECommerceApp.Core.Extensions;
using ECommerceApp.Core.Helpers;
using MongoDB.Bson;
using ECommerceApp.Infrastructure.Mappings;
using ECommerceApp.Infrastructure.Mappings.Product;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json.Linq;

namespace ECommerceApp.Application.Services.Concrete;

public class CategoryService : CRUDService<IRepository<Category, ObjectId>, Category, ObjectId>, ICategoryService
{
    public CategoryService(IRepository<Category, ObjectId> repository) : base(repository)
    {

    }

    public async Task<PaginatedList<CategoryDTO>> Filter(int page, int pageSize, GridFilters? filter = null)
    {
        var allData = GetAll();

        if (filter != null && filter.Filters != null && filter.Filters.Any() && !string.IsNullOrEmpty(filter.Logic))
        {
            foreach (var field in filter.Filters)
            {
                if (field.Field == "name")
                    allData = allData.Where(x => x.CategoryLanguages.Any(cl => cl.Name.Contains(field.Value)));
                else if (field.Field == "isActive")
                    allData = allData.Where(x => x.IsActive == bool.Parse(field.Value));
            }
        }

        var categoriesAsJson = allData.ToJson();
        var deserializedCategories = BsonSerializer.Deserialize<List<CategoryDTO>>(categoriesAsJson);
        var pagedData = await deserializedCategories.AsQueryable().ToPagedListAsync(page, pageSize, typeof(IMongoQueryable));
        return pagedData;
    }

}

