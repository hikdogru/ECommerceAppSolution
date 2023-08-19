using AutoMapper;
using ECommerceApp.Core.Domain.Entities;
using ECommerceApp.Core.DTOs;

namespace ECommerceApp.Infrastructure.Mappings.Product;

public class CategoryMapping : Profile
{

    public CategoryMapping()
    {
        //CreateMap<Category, CategoryDTO>()
        //    .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
        //    .ForMember(dest => dest.Name, act => act.MapFrom(src => src.CategoryLanguages.FirstOrDefault(l => !string.IsNullOrEmpty(l.Name)).Name))
        //    .ForMember(dest => dest.IsActive, act => act.MapFrom(src => src.IsActive))
        //    .ForMember(dest => dest.MediaPath, act => act.MapFrom(src => src.CategoryMedias.FirstOrDefault(m => !string.IsNullOrEmpty(m.Path)).Path));


    }
}


