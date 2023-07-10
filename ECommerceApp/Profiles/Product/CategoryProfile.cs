using AutoMapper;
using ECommerceApp.Core.Domain.Entities;
using ECommerceApp.WebUI.Models;

namespace ECommerceApp.WebUI.Profiles.Product;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryModel>().ReverseMap();
    }
}