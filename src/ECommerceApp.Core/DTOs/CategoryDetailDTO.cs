using ECommerceApp.Core.Domain.Entities.Product;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.DTOs;

[BsonIgnoreExtraElements]
public record CategoryDetailDTO(ObjectId Id, List<CategoryLanguage> CategoryLanguages, bool IsActive, ObjectId? ParentId,
    List<CategoryMedia>? CategoryMedias)
{

};