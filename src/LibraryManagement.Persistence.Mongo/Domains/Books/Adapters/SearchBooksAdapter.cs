using System.Linq.Expressions;

using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Search;

using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.Domains.Books.Adapters;

public class SearchBooksAdapter(IBookCollection bookCollection, IBookEntityMapper mapper) : ISearchBooksPort
{
    public async Task<SearchResult<Book>> Search(string? searchTerm, Pagination pagination)
    {
        Expression<Func<BookEntity, bool>> filter = book =>
            searchTerm == null ||
            book.Title.Contains(searchTerm);

        var count = await bookCollection.Collection.CountDocumentsAsync(filter);

        IAsyncCursor<BookEntity>? searchRequest = await bookCollection.Collection.FindAsync(filter
            , new FindOptions<BookEntity> { Skip = 0, Limit = 10 });

        return new()
        {
            Results = searchRequest.ToEnumerable().Select(mapper.ToDomain).ToList(),
            Pagination = new()
            {
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize,
                TotalItems = count
            }
        };
    }
}
