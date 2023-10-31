using AutoMapper;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.DTOs;

namespace ECommerceApp.Infrastructure.Mappings.Product;

public class CategoryMapping : Profile
{

    public CategoryMapping()
    {
        CreateMap<Category, CategoryDTO>();
    }
}


