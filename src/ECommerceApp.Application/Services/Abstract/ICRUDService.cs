using ECommerceApp.Core.Domain.Interfaces;
using MongoDB.Bson;

namespace ECommerceApp.Application.Services.Abstract;

public interface ICRUDService<TEntity, TKey> where TEntity : IEntity<TKey>
{
    IQueryable<TEntity> GetAll();
    Task<TEntity> GetById(ObjectId id);
    Task Delete(TEntity entity, bool isBulk = false);
    Task Delete(TKey id);
    Task Insert(TEntity entity, bool isBulk = false);
    Task Update(TEntity entity, bool isBulk = false);
}