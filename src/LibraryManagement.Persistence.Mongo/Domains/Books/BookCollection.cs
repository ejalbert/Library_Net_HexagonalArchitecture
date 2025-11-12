using LibraryManagement.Persistence.Mongo.Abstractions;

using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.Domains.Books;


public interface IBookCollection : IAbstractCollection<BookEntity>;

public class BookCollection : AbstractCollection<BookEntity>, IBookCollection
{
    public BookCollection(IMongoDatabase database)
        : base(database.GetCollection<BookEntity>("books"))
    {
        EnsureIndexes();
    }

    private void EnsureIndexes()
    {
        IndexKeysDefinition<BookEntity> titleIndex = Builders<BookEntity>.IndexKeys.Ascending(book => book.Title);

        Collection.Indexes.CreateOne(
            new CreateIndexModel<BookEntity>(titleIndex, new CreateIndexOptions { Name = "idx_books_title" }));
    }
}
