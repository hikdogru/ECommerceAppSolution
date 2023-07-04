using ECommerceApp.Core.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.Domain;

public class Document : IEntity<ObjectId>
{
    [BsonId]
    [BsonIgnoreIfDefault]
    public ObjectId Id { get; set; }
}