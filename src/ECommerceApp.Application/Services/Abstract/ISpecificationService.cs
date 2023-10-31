using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.DTOs;
using MongoDB.Bson;

namespace ECommerceApp.Application.Services.Abstract;

public interface ISpecificationService : ICRUDService<Specification, ObjectId>
{
    Task<PaginatedList<SpecificationDTO>> Filter(int page, int pageSize, GridFilters? filter = null);
}
