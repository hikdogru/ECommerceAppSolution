using ECommerceApp.Core.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace ECommerceApp.Core.Helpers;

public class MongoDbPaginatedList<T> : PaginatedList<T>
{
    public MongoDbPaginatedList(IQueryable<T> items)
    {
        source = items;
    }

    public override async Task<PaginatedList<T>> CreateAsync(int page, int pageSize)
    {
        try
        {
            var count = await ((IMongoQueryable<T>)source).CountAsync();
            CurrentPage = page;
            TotalPages = (int)System.Math.Ceiling(count / (double)pageSize);
            var items = await ((IMongoQueryable<T>)source).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            AddRange(items.ToList());
            return this;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

