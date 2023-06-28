using ECommerceApp.Core.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.WebUI.Models;

public class CategoryModel
{
    /// <summary>
    /// Category Id
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

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
    public List<CategoryLanguageModel> CategoryLanguages { get; set; }

    // Category Medias
    public List<CategoryMediaModel> CategoryMedias { get; set; }
}
