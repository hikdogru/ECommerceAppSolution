using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.Domain;

public class DocumentShortTrack : Document
{
    public string CreatedUser { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime CreatedDate { get; set; }
}