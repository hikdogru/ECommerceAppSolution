using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.WebUI.Models.Category;

public class CategoryLanguageModel
{
    /// <summary>
    /// Category Language Id
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    // Language Id
    public string LanguageId { get; set; }

    // Language Code
    public string LanguageCode { get; set; }

    // Category Name
    public string Name { get; set; }

    // Category Description
    public string? Description { get; set; }

    // Category Sort Number
    public int SortNr { get; set; }
}