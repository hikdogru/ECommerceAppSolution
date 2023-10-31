using AutoMapper;
using ECommerceApp.Core.DTOs;
using ECommerceApp.Core.Domain.Entities.Product;

namespace ECommerceApp.Infrastructure.Mappings.Product;

public class ProductMapping : Profile
{
    public ProductMapping()
    {
        CreateMap<Core.Domain.Entities.Product.Product, ProductDTO>();
    }
}
