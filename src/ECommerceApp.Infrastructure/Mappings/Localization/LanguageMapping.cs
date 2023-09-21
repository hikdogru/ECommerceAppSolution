using AutoMapper;
using ECommerceApp.Core.Domain.Entities.Language;
using ECommerceApp.Core.DTOs;

namespace ECommerceApp.Infrastructure.Mappings.Localization;

public class LanguageMapping : Profile
{
    public LanguageMapping()
    {
        CreateMap<Language, LanguageDTO>();
    }
}