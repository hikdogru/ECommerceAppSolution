using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.DTOs;

[BsonIgnoreExtraElements]
public record LanguageDTO(ObjectId Id, string Name, string Code, string Direction, bool IsActive);