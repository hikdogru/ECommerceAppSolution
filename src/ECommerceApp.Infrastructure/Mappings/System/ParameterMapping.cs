using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerceApp.Core.Domain.Entities.System;
using ECommerceApp.Core.DTOs;

namespace ECommerceApp.Infrastructure.Mappings.System;

public class ParameterMapping : Profile
{
    public ParameterMapping()
    {
        CreateMap<Parameter, ParameterDTO>();
    }
}
