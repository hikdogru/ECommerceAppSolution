using ECommerceApp.Core.Domain.Entities;
using MongoDB.Bson.Serialization;

namespace ECommerceApp.Infrastructure.Persistence;

public class CategoryMap
{
    public static void Configure()
    {
        BsonClassMap.RegisterClassMap<Category>(map =>
        {
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
            map.MapIdMember(x => x.Id);
        });
    }
}