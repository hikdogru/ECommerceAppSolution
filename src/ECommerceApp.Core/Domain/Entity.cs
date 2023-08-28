using System.ComponentModel.DataAnnotations.Schema;
using ECommerceApp.Core.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace ECommerceApp.Core.Domain;

public class Entity : IEntity<ObjectId>
{
    [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
    public ObjectId Id { get; set; }
}