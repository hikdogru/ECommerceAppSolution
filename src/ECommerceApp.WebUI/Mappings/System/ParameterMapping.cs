
using AutoMapper;
using ECommerceApp.Core.Domain.Entities.System;
using ECommerceApp.Core.DTOs;
using ECommerceApp.WebUI.Areas.Admin.Models.System;

namespace ECommerceApp.WebUI.Mappings.System;

public class ParameterMapping : Profile
{
    public ParameterMapping()
    {
        CreateMap<ParameterDTO, ParameterViewModel>();
        CreateMap<ParameterModel, Parameter>().ReverseMap();
    }
}
