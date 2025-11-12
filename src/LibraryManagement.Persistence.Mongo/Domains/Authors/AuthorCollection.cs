using LibraryManagement.Persistence.Mongo.Abstractions;

using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.Domains.Authors;

public interface IAuthorCollection : IAbstractCollection<AuthorEntity>;

public class AuthorCollection : AbstractCollection<AuthorEntity>, IAuthorCollection
{
    public AuthorCollection(IMongoDatabase database)
        : base(database.GetCollection<AuthorEntity>("authors"))
    {
        EnsureIndexes();
    }

    private void EnsureIndexes()
    {
        IndexKeysDefinition<AuthorEntity> nameIndex = Builders<AuthorEntity>.IndexKeys.Ascending(author => author.Name);

        Collection.Indexes.CreateOne(
            new CreateIndexModel<AuthorEntity>(nameIndex, new CreateIndexOptions { Name = "idx_authors_name" }));
    }
}
