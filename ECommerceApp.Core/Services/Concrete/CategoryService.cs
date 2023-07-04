using ECommerceApp.Core.Domain.Entities;
using ECommerceApp.Core.Domain.Interfaces.Repository;
using ECommerceApp.Core.Services.Abstract;
using MongoDB.Bson;

namespace ECommerceApp.Core.Services.Concrete;

public class CategoryService : CRUDService<IRepository<Category, ObjectId>, Category, ObjectId>, ICategoryService
{
    public CategoryService(IRepository<Category, ObjectId> repository) : base(repository)
    {
    }
}