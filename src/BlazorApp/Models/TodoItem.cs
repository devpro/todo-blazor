using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Devpro.TodoList.BlazorApp.Models;

public class TodoItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty; // Link to ApplicationUser.Id.ToString()

    public string Title { get; set; } = string.Empty;

    [BsonElement("is_done")]
    public bool IsDone { get; set; }
}
