using ECommerceApp.Core.Domain.Interfaces;

namespace ECommerceApp.Core.Domain;

public abstract class PaginatedList<T> : List<T>
{
    public int CurrentPage { get; protected set; }
    public int TotalPages { get; protected set; }
    internal IQueryable<T> source;

    public bool HasPreviousPage
    {
        get
        {
            return (CurrentPage > 1);
        }
    }

    public bool HasNextPage
    {
        get
        {
            return (CurrentPage < TotalPages);
        }
    }

    public abstract Task<PaginatedList<T>> CreateAsync(int page, int pageSize);
}