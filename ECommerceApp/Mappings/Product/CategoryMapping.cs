using AutoMapper;
using ECommerceApp.Core.Domain.Entities;
using ECommerceApp.WebUI.Models;
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

        CreateMap<CategoryLanguage, CategoryLanguageModel>().ReverseMap();
        CreateMap<CategoryMedia, CategoryMediaModel>().ReverseMap();
    }
}