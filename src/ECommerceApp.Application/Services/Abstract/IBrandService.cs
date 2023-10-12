using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.DTOs;
using MongoDB.Bson;

namespace ECommerceApp.Application.Services.Abstract;

public interface IBrandService : ICRUDService<Brand, ObjectId>
{
    Task<PaginatedList<BrandDTO>> Filter(int page, int pageSize, GridFilters? filter = null);
}
