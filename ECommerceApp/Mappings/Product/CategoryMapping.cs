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
        CreateMap<Category, CategoryModel>().ReverseMap();
        CreateMap<CategoryLanguage, CategoryLanguageModel>().ReverseMap();
        CreateMap<CategoryMedia, CategoryMediaModel>().ReverseMap();
    }
}