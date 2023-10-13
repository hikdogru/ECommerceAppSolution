using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.DTOs;

namespace ECommerceApp.Infrastructure.Mappings.Product;

public class TagMapping: Profile
{
    public TagMapping()
    {
        CreateMap<Tag, TagDTO>();
    }
}
