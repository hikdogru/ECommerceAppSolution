using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Core.Domain;

public class DocumentLongTrack : DocumentShortTrack
{
    public string UpdatedUser { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime UpdatedDate { get; set; }

    public bool IsDeleted { get; set; } = false;
}