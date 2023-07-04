using ECommerceApp.Core.Domain.Interfaces;

namespace ECommerceApp.Core.Services.Abstract;

public interface ICRUDService<TEntity, TKey> where TEntity : IEntity<TKey>
{
    IQueryable<TEntity> GetAll();
    Task Delete(TEntity entity, bool isBulk = false);
    Task Delete(TKey id);
    Task Insert(TEntity entity, bool isBulk = false);
    Task Update(TEntity entity, bool isBulk = false);
}