using AutoMapper;
using ECommerceApp.Core.Domain.Entities;
using ECommerceApp.Core.DTOs;
using ECommerceApp.WebUI.Models.Category;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace ECommerceApp.WebUI.Mappings.Product;

public class CategoryMapping : Profile
{
    public CategoryMapping()
    {
        CreateMap<Category, CategoryModel>();
        CreateMap<CategoryModel, Category>().ForMember(dest => dest.CategoryMedias,
                act => act.MapFrom(src =>
                    src.CategoryMedias.Where(m => m.Path != null && m.MediaType != null && m.SizeType != null)))
            .ForMember(dest => dest.CategoryLanguages,
                act => act.MapFrom(src =>
                    src.CategoryLanguages.Where(m =>
                        !string.IsNullOrEmpty(m.Name) && !string.IsNullOrEmpty(m.LanguageId))));


        CreateMap<CategoryDTO, CategoryViewModel>()
            .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, act => act.MapFrom(src => src.CategoryLanguages.FirstOrDefault(l => !string.IsNullOrEmpty(l.Name)).Name))
            .ForMember(dest => dest.IsActive, act => act.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.MediaPath, act => act.MapFrom(src => src.CategoryMedias.FirstOrDefault(m => !string.IsNullOrEmpty(m.Path)).Path));


        CreateMap<CategoryLanguage, CategoryLanguageModel>().ReverseMap();
        CreateMap<CategoryMedia, CategoryMediaModel>().ReverseMap();
    }
}