using ECommerceApp.Core.Domain.Entities.Product;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

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
