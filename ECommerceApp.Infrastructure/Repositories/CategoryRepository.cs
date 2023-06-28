using ECommerceApp.Core.Domain.Entities;
using ECommerceApp.Core.Domain.Interfaces;
using ECommerceApp.Core.Domain.Interfaces.Repository;

namespace ECommerceApp.Infrastructure.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(IMongoContext context) : base(context)
    {
    }
}