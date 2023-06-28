namespace ECommerceApp.Core.Domain.Interfaces.Repository;

public interface IRepository<TEntity> : IDisposable where TEntity : class
{
    Task Add(TEntity obj);
    Task<TEntity> GetById(Guid id);
    Task<IEnumerable<TEntity>> GetAll();
    void Update(TEntity obj);
    void Remove(Guid id);
    Task SaveChanges();
}