using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.Domain.Entities.Product;

public class SpecificationValue : DocumentLongTrack
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("SpecificationId")]
    public string SpecificationId { get; set; }

    /// <summary>
    /// Gets or sets the list of specification value languages.
    /// </summary>
    public List<SpecificationValueLanguage> SpecificationValueLanguages { get; set; }
}


