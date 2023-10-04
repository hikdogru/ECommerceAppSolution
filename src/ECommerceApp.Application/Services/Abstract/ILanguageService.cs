using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.Language;
using ECommerceApp.Core.DTOs;
using MongoDB.Bson;

namespace ECommerceApp.Application.Services.Abstract;

public interface ILanguageService : ICRUDService<Language, ObjectId>
{
    Task<PaginatedList<LanguageDTO>> Filter(int page, int pageSize, GridFilters? filter = null);
    IQueryable<LanguageDTO> GetAllIfIsNotDeleted();
    IQueryable<LanguageDTO> GetAllIfActive();

}