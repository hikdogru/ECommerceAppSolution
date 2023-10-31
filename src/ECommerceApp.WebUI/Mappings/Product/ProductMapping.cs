using AutoMapper;
using ECommerceApp.Core.DTOs;
using ECommerceApp.WebUI.Areas.Admin.Models.Catalog;
using System.Linq;

namespace ECommerceApp.WebUI.Mappings.Product;

public class ProductMapping : Profile
{
    public ProductMapping()
    {
        CreateMap<ProductDTO, ProductViewModel>().ForMember(dest => dest.Image,
        act => act.MapFrom(src => src.Images.FirstOrDefault(x => x != null && x.IsDefault).Path))
        .ForMember(dest => dest.TotalQuantity, act => act.MapFrom(src => src.ProductVariants.Sum(x => x.Quantity)))
        .ForMember(dest => dest.Name, act =>
        act.MapFrom(src => src.ProductLanguages.FirstOrDefault(l => !string.IsNullOrEmpty(l.Name)).Name));
    }
}

