using ECommerceApp.Core.Domain.Entities.Language;
using MongoDB.Bson;

namespace ECommerceApp.Application.Services.Abstract;

public interface IDictionaryService : ICRUDService<Dictionary, ObjectId>
{

}