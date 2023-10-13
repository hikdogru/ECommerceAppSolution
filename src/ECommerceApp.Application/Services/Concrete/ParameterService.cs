using AutoMapper.QueryableExtensions;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Interfaces.Repository;
using ECommerceApp.Core.DTOs;
using ECommerceApp.Core.Extensions;
using ECommerceApp.Infrastructure.Mappings;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Text.RegularExpressions;
using ECommerceApp.Core.Domain.Entities.System;

namespace ECommerceApp.Application.Services.Concrete;

public class ParameterService : CRUDService<IRepository<Parameter, ObjectId>, Parameter, ObjectId>, IParameterService
{
    public ParameterService(IRepository<Parameter, ObjectId> repository) : base(repository)
    {
    }

    public async Task<PaginatedList<ParameterDTO>> Filter(int page, int pageSize, GridFilters? filter = null)
    {
        var allData = GetAll().Where(x => !x.IsDeleted);

        if (filter != null && filter.Filters != null && filter.Filters.Any() && !string.IsNullOrEmpty(filter.Logic))
        {
            foreach (var field in filter.Filters)
            {
                if (field.Field == "name" || field.Field == "value")
                    allData = allData.Where(x => Regex.IsMatch(x.Name, field.Value, RegexOptions.IgnoreCase) ||
                                                 Regex.IsMatch(x.Value, field.Value, RegexOptions.IgnoreCase));

            }
        }

        var pagedData = await allData.ProjectTo<ParameterDTO>(MapperConfig.GetMapper().ConfigurationProvider)
            .ToPagedListAsync(page, pageSize, typeof(IMongoQueryable));

        return pagedData;
    }
}
