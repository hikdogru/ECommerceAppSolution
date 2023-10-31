using AutoMapper;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.DTOs;

namespace ECommerceApp.Infrastructure.Mappings.Product;

public class SpecificationMapping : Profile
{
    public SpecificationMapping()
    {
        CreateMap<Specification, SpecificationDTO>();
    }
}
