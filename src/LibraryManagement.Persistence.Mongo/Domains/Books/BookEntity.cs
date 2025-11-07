using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LibraryManagement.Persistence.Mongo.Domains.Books;

public class BookEntity
{
    [BsonId] public string Id { get; init; } = ObjectId.GenerateNewId().ToString();
    
    public string Title { get; set; }
}