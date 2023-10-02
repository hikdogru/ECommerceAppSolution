using ECommerceApp.Application.Models;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.DTOs;
using MongoDB.Bson;

namespace ECommerceApp.Application.Services.Abstract;

public interface ICategoryService : ICRUDService<Category, ObjectId>
{
    Task<PaginatedList<CategoryDTO>> Filter(int page, int pageSize, GridFilters? filter = null);
    Task<CategoryDetailDTO> GetCategoryById(ObjectId id);
    List<CategoryTreeArrowModel> GetCategoryTree(string languageCode = "en", string seperator = " > ");
    List<CategoryHierarchicalModel> GetHierarchicals(ObjectId? parent = null);
}