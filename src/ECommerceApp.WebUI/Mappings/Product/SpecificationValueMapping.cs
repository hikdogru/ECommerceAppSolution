using AutoMapper;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.DTOs;
using ECommerceApp.WebUI.Areas.Admin.Models.Catalog;

namespace ECommerceApp.WebUI.Mappings.Product;

public class SpecificationValueMapping : Profile
{
    public SpecificationValueMapping()
    {
        CreateMap<SpecificationValueDTO, SpecificationValueViewModel>();
        CreateMap<SpecificationValueModel, SpecificationValue>().ReverseMap();
        CreateMap<SpecificationValueLanguageModel, SpecificationValueLanguage>().ReverseMap();
    }
}
