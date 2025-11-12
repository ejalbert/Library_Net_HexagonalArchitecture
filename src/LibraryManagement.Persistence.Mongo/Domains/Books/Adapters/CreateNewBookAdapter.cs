using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Create;

namespace LibraryManagement.Persistence.Mongo.Domains.Books.Adapters;

public class CreateNewBookAdapter(IBookCollection bookCollection, IBookEntityMapper bookEntityMapper) : ICreateNewBookPort
{
    public async Task<Book> Create(string title, string authorId)
    {
        BookEntity bookEntity = new() { Title = title, AuthorId = authorId };

        await bookCollection.Collection.InsertOneAsync(bookEntity);

        return bookEntityMapper.ToDomain(bookEntity);
    }
}
