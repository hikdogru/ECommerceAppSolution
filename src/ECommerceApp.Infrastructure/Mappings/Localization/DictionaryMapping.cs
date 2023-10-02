using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerceApp.Core.Domain.Entities.Language;
using ECommerceApp.Core.DTOs;

namespace ECommerceApp.Infrastructure.Mappings.Localization
{
    public class DictionaryMapping : Profile
    {
        public DictionaryMapping()
        {
            CreateMap<Dictionary, DictionaryDTO>();
        }
    }
}