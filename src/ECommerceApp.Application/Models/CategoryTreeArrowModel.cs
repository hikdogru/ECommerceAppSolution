using MongoDB.Bson;

namespace ECommerceApp.Application.Models;

public class CategoryTreeArrowModel
{
    public ObjectId Id { get; set; }
    public string? Text { get; set; }
}