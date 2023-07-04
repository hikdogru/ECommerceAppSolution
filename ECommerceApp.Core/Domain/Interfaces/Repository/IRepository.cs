namespace ECommerceApp.Core.Domain.Interfaces.Repository;

public interface IRepository<TEntity, TKey> : IDisposable where TEntity : IEntity<TKey>
{
    Task Add(TEntity obj);
    Task<TEntity> GetById(Guid id);
    IQueryable<TEntity> GetAll();
    Task Update(TEntity obj);
    Task Remove(TKey id);
    Task Remove(TEntity obj);
    Task SaveChanges();
}