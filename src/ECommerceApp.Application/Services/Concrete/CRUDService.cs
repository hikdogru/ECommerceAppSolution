using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain.Interfaces;
using ECommerceApp.Core.Domain.Interfaces.Repository;
using MongoDB.Bson;

namespace ECommerceApp.Application.Services.Concrete;

public class CRUDService<TRepository, TEntity, TKey> : ICRUDService<TEntity, TKey> where TEntity : IEntity<TKey>
    where TRepository : IRepository<TEntity, TKey>
{
    protected readonly TRepository Repository;

    public CRUDService(TRepository repository)
    {
        Repository = repository;
    }


    public virtual async Task Add(TEntity entity)
    {
        await Repository.Add(entity);
    }

    public async Task Insert(TEntity entity, bool isBulk = false)
    {
        await Repository.Add(entity);
    }

    public virtual async Task Update(TEntity entity, bool isBulk = false)
    {
        await Repository.Update(entity);
    }


    public virtual IQueryable<TEntity> GetAll()
    {
        return Repository.GetAll();
    }

    public virtual async Task<TEntity> GetById(ObjectId id)
    {
        return await Repository.GetById(id);
    }

    public virtual async Task Delete(TEntity entity, bool isBulk = false)
    {
        await Repository.Remove(entity);
    }

    public virtual async Task Delete(TKey id)
    {
        await Repository.Remove(id);
    }
}