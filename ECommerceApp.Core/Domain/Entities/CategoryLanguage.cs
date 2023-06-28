using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ECommerceApp.Core.Domain.Entities;

public class CategoryLanguage
{
    public CategoryLanguage()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    /// <summary>
    /// Category Language Id
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    // Language Id
    public string LanguageId { get; set; }

    // Language Code
    public string LanguageCode { get; set; }

    // Category Name
    public string Name { get; set; }

    // Category Description
    public string Description { get; set; }

    // Category Sort Number
    public int SortNr { get; set; }
}