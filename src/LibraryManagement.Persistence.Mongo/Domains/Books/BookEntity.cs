using System.Collections.Generic;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LibraryManagement.Persistence.Mongo.Domains.Books;

public class BookEntity
{
    [BsonId] public string Id { get; init; } = ObjectId.GenerateNewId().ToString();

    public string Title { get; set; } = string.Empty;

    public string AuthorId { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<string> Keywords
    {
        get;
        set => field = value ?? [];
    } = [];
}
