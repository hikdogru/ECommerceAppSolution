﻿using ECommerceApp.Core.Domain.Entities.Product;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.DTOs;

[BsonIgnoreExtraElements]
public record CategoryDTO(string Id, List<CategoryLanguage> CategoryLanguages, bool IsActive, List<CategoryMedia>? CategoryMedias)
{

};





