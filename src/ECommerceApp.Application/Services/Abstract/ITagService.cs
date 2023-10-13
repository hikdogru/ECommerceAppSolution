using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.DTOs;
using MongoDB.Bson;

namespace ECommerceApp.Application.Services.Abstract;

public interface ITagService : ICRUDService<Tag, ObjectId>
{
    Task<PaginatedList<TagDTO>> Filter(int page, int pageSize, GridFilters? filter = null);
}
