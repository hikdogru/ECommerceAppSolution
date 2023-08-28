using System.Collections.Generic;
using System.Linq;
using ECommerceApp.Core.Domain;


namespace ECommerceApp.Core.Helpers;

public class EntityFrameworkPaginatedList<T> : PaginatedList<T>
{

    public EntityFrameworkPaginatedList(IQueryable<T> items)
    {
        source = items;
    }

    public EntityFrameworkPaginatedList<T> Create(int page, int pageSize)
    {
        var count = source.Count();
        CurrentPage = page;
        TotalPages = (int)System.Math.Ceiling(count / (double)pageSize);
        var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        AddRange(items.ToList());
        return this;
    }

    public override Task<PaginatedList<T>> CreateAsync(int page, int pageSize)
    {
        throw new NotImplementedException();
    }
}

