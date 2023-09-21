using AutoMapper;
using ECommerceApp.Core.Domain.Entities.Language;
using ECommerceApp.Core.DTOs;
using ECommerceApp.WebUI.Areas.Admin.Models.Localization;

namespace ECommerceApp.WebUI.Mappings.Localization;

public class LanguageMapping : Profile
{
    public LanguageMapping()
    {
        CreateMap<LanguageDTO, LanguageViewModel>();
        CreateMap<LanguageModel, Language>().ReverseMap();
    }
}