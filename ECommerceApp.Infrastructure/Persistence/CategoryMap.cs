using ECommerceApp.Core.Domain.Entities;
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
            map.MapMember(c => c.CategoryLanguages)
                .SetSerializer(new CustomCategoryLanguageSerializer());
            map.MapMember(c => c.CategoryMedias)
                .SetSerializer(new CustomCategoryMediaSerializer());
        });
    }
}

public class CustomCategoryLanguageSerializer : SerializerBase<List<CategoryLanguage>>
{
    public override List<CategoryLanguage> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonReader = context.Reader;

        // Assuming the nested collection is an array of documents
        var array = BsonArraySerializer.Instance.Deserialize(context);

        var result = array.Select(item => item["Name"].AsString)
            .Where(name => !string.IsNullOrEmpty(name))
            .Select(name => new CategoryLanguage { Name = name })
            .ToList();

        return result;
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, List<CategoryLanguage> value)
    {
        // Implement serialization logic here.
        throw new NotImplementedException();
    }
}

// Custom serializer for projecting CategoryMedia properties
public class CustomCategoryMediaSerializer : SerializerBase<List<CategoryMedia>>
{
    public override List<CategoryMedia> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonReader = context.Reader;

        // Assuming the nested collection is an array of documents
        var array = BsonArraySerializer.Instance.Deserialize(context);

        var result = array.Select(item => item["Path"].AsString)
            .Where(path => !string.IsNullOrEmpty(path))
            .Select(path => new CategoryMedia { Path = path })
            .ToList();

        return result;
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, List<CategoryMedia> value)
    {
        // Implement serialization logic here.
        throw new NotImplementedException();
    }
}