namespace ECommerceApp.Core.Domain.Interfaces;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}