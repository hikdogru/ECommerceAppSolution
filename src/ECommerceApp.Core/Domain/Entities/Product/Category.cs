﻿using ECommerceApp.Core.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.Domain.Entities.Product;

[BsonIgnoreExtraElements]
public class Category : DocumentLongTrack
{
    /// <summary>
    /// Category ParentId
    /// </summary>
    [BsonRepresentation(BsonType.ObjectId)]
    public string ParentId { get; set; }

    /// <summary>
    /// Is Category Active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Category Languages
    /// </summary>
    public List<CategoryLanguage> CategoryLanguages { get; set; }

    // Category Medias
    public List<CategoryMedia> CategoryMedias { get; set; }
}






