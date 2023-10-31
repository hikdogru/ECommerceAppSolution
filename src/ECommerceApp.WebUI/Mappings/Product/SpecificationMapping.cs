using AutoMapper;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.DTOs;
using ECommerceApp.WebUI.Areas.Admin.Models.Catalog;

namespace ECommerceApp.WebUI.Mappings.Product;

public class SpecificationMapping : Profile
{
   
    public SpecificationMapping()
    {
        CreateMap<SpecificationDTO, SpecificationViewModel>();                
        CreateMap<SpecificationModel, Specification>().ReverseMap();
        CreateMap<SpecificationLanguageModel, SpecificationLanguage>().ReverseMap();
    }
}
