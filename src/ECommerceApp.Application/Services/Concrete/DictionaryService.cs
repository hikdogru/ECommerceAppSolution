using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain.Entities.Language;
using ECommerceApp.Core.Domain.Interfaces.Repository;
using MongoDB.Bson;

namespace ECommerceApp.Application.Services.Concrete;

public class DictionaryService : CRUDService<IRepository<Dictionary, ObjectId>, Dictionary, ObjectId>, IDictionaryService
{
    public DictionaryService(IRepository<Dictionary, ObjectId> repository) : base(repository)
    {
    }
}