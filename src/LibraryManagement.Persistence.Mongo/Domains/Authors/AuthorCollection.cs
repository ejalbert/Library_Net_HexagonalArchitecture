using LibraryManagement.Persistence.Mongo.Abstractions;

using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.Domains.Authors;

public interface IAuthorCollection : IAbstractCollection<AuthorEntity>;

public class AuthorCollection(IMongoDatabase database)
    : AbstractCollection<AuthorEntity>(database.GetCollection<AuthorEntity>("authors")), IAuthorCollection;
