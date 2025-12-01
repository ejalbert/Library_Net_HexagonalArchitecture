using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Search;

using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.Domains.Books.Adapters;

public class SearchBooksAdapter(IBookCollection bookCollection, IBookEntityMapper mapper) : ISearchBooksPort
{
    public async Task<IEnumerable<Book>> Search(string? searchTerm, Pagination pagination)
    {
        IAsyncCursor<BookEntity>? searchRequest = await bookCollection.Collection.FindAsync(book =>
                searchTerm == null ||
                book.Title.Contains(searchTerm)
            , new FindOptions<BookEntity> { Skip = 0, Limit = 10 });

        return searchRequest.ToEnumerable().Select(mapper.ToDomain);
    }
}
