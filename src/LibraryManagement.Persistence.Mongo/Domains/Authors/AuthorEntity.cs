using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LibraryManagement.Persistence.Mongo.Domains.Authors;

public class AuthorEntity
{
    [BsonId] public string Id { get; init; } = ObjectId.GenerateNewId().ToString();

    public string Name { get; set; } = string.Empty;
}
