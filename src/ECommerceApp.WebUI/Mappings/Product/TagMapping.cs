using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.DTOs;
using ECommerceApp.WebUI.Areas.Admin.Models.Catalog;

namespace ECommerceApp.WebUI.Mappings.Product;

public class TagMapping : Profile
{
    public TagMapping()
    {
        CreateMap<TagDTO, TagViewModel>();
        CreateMap<TagModel, Tag>().ReverseMap();
    }
}
