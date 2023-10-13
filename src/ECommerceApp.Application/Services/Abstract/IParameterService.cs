using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities.System;
using ECommerceApp.Core.DTOs;
using MongoDB.Bson;

namespace ECommerceApp.Application.Services.Abstract;

public interface IParameterService : ICRUDService<Parameter, ObjectId>
{
    Task<PaginatedList<ParameterDTO>> Filter(int page, int pageSize, GridFilters? filter = null);
}
