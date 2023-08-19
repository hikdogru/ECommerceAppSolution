using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Helpers;
using MongoDB.Driver.Linq;

namespace ECommerceApp.Core.Extensions;

public static class ModelExtensions
{
    public static async Task<PaginatedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int page, int pageSize, Type type)
    {
        if (type.Name is "IMongoQueryable")
        {
            var model = new MongoDbPaginatedList<T>(source);
            var paginatedList = await model.CreateAsync(page, pageSize);
            return paginatedList;
        }
        else if (source is IQueryable<T> efSource)
        {
            var model = new EntityFrameworkPaginatedList<T>(efSource);
            return await model.CreateAsync(page, pageSize);
        }
        else
        {
            throw new ArgumentException("The queryable source must be either an EF or MongoDB source.");
        }
    }


}