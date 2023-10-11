using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.Language;
using ECommerceApp.Core.DTOs;
using MongoDB.Bson;

namespace ECommerceApp.Application.Services.Abstract;

public interface IDictionaryService : ICRUDService<Dictionary, ObjectId>
{
    Task<PaginatedList<DictionaryDTO>> Filter(int page, int pageSize, GridFilters? filter = null);
    string GetWord(string key, ObjectId? languageId = null);
}