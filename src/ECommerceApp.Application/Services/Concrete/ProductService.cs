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

public class ProductService : CRUDService<IRepository<Product, ObjectId>, Product, ObjectId>, IProductService
{
    public ProductService(IRepository<Product, ObjectId> repository) : base(repository)
    {
    }

    public async Task<PaginatedList<ProductDTO>> Filter(int page, int pageSize, GridFilters? filter = null)
    {
        var allData = GetAll().Where(x => !x.IsDeleted);

        if (filter != null && filter.Filters != null && filter.Filters.Any() && !string.IsNullOrEmpty(filter.Logic))
        {
            foreach (var field in filter.Filters)
            {
                if (field.Field == "name")
                    allData = allData.Where(x => x.ProductLanguages.Any(cl => Regex.IsMatch(cl.Name, field.Value, RegexOptions.IgnoreCase)));

                else if (field.Field == "isItOffSale")
                    allData = allData.Where(x => x.IsItOffSale == bool.Parse(field.Value));
            }
        }

        var pagedData = await allData.ProjectTo<ProductDTO>(MapperConfig.GetMapper().ConfigurationProvider)
            .ToPagedListAsync(page, pageSize, typeof(IMongoQueryable));

        return pagedData;
    }
}
