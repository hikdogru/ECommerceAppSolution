using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.Domain.Entities.System;

/// <summary>
/// Represents a system parameter with a name and a value.
/// </summary>
public class Parameter : DocumentLongTrack
{
    // Gets or sets the name of the parameter.    
    [BsonElement("Name")]
    public string Name { get; set; }


    // Gets or sets the value of the parameter.
    [BsonElement("Value")]
    public string Value { get; set; }
}
