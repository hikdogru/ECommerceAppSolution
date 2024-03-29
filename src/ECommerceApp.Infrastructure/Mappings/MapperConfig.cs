﻿using System.Collections;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ECommerceApp.Infrastructure.Mappings.Localization;
using ECommerceApp.Infrastructure.Mappings.Product;
using ECommerceApp.Infrastructure.Mappings.System;
using MongoDB.Driver.Linq;

namespace ECommerceApp.Infrastructure.Mappings;

public static class MapperConfig
{
    public static IMapper GetMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {            
            cfg.AddMaps(typeof(ParameterMapping));
        });
        var mapper = config.CreateMapper();
        return mapper;
    }

    public static IQueryable<TTarget> MapTo<TSource, TTarget>(this IQueryable source)
    {
        try
        {
            var mapper = GetMapper();
            return mapper.ProjectTo<TTarget>(source);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static IMongoQueryable<TDestination> ProjectTo<TSource, TDestination>(this IQueryable<TSource> query)
    {
        var mapper = GetMapper();
        return query.ProjectTo<TDestination>(mapper.ConfigurationProvider) as IMongoQueryable<TDestination>;
    }
}