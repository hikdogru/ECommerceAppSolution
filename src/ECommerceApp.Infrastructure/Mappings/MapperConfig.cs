using System.Collections;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ECommerceApp.Infrastructure.Mappings.Localization;
using ECommerceApp.Infrastructure.Mappings.Product;
using MongoDB.Driver.Linq;

namespace ECommerceApp.Infrastructure.Mappings;

public static class MapperConfig
{
    public static IMapper GetMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CategoryMapping>();
            cfg.AddProfile<LanguageMapping>();
            cfg.AddProfile<DictionaryMapping>();
            cfg.AddProfile<BrandMapping>();
            cfg.AddProfile<TagMapping>();
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