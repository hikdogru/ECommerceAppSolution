using AutoMapper;
using ECommerceApp.Core.Domain.Entities.Language;
using ECommerceApp.Core.DTOs;
using ECommerceApp.WebUI.Areas.Admin.Models.Localization;

namespace ECommerceApp.WebUI.Mappings.Localization
{
    public class DictionaryMapping : Profile
    {

        public DictionaryMapping()
        {
            CreateMap<DictionaryDTO, DictionaryViewModel>();
            CreateMap<DictionaryModel, Dictionary>().ReverseMap();
        }
    }
}