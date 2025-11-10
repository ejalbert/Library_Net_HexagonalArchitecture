using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.GetSingleBook;
using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.Domains.Books.Adapters;

public class GetSingleBookAdapter(IBookCollection bookCollection, IBookEntityMapper mapper) : IGetSingleBookPort
{
    public async Task<Book> GetById(string id)
    {
        var cursor = await bookCollection.Collection.FindAsync(it => it.Id == id);

        return mapper.ToDomain(cursor.Single());
    }
}