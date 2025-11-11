using LibraryManagement.Persistence.Mongo.Abstractions;

using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.Domains.Books;


public interface IBookCollection : IAbstractCollection<BookEntity>;

public class BookCollection(IMongoDatabase database)
    : AbstractCollection<BookEntity>(database.GetCollection<BookEntity>("books")), IBookCollection;
