using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.DTOs;

[BsonIgnoreExtraElements]
public record ParameterDTO(ObjectId Id, string Name, string Value);
