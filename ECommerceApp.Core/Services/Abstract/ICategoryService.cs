using ECommerceApp.Core.Domain.Entities;
using ECommerceApp.Core.Domain.Interfaces.Repository;
using MongoDB.Bson;

namespace ECommerceApp.Core.Services.Abstract;

public interface ICategoryService : ICRUDService<Category, ObjectId>
{

}