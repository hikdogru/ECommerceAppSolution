using ECommerceApp.Application.Models;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities;
using ECommerceApp.Core.DTOs;
using MongoDB.Bson;

namespace ECommerceApp.Application.Services.Abstract;

public interface ICategoryService : ICRUDService<Category, ObjectId>
{
    Task<PaginatedList<CategoryDTO>> Filter(int page, int pageSize, GridFilters? filter = null);
    List<CategoryTreeArrowModel> GetCategoryTree(string languageCode = "English", string seperator = " > ");
}