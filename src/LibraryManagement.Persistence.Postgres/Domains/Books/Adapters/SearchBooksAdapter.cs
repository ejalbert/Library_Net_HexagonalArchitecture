using System.Linq.Expressions;

using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Search;
using LibraryManagement.Persistence.Postgres.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Domains.Books.Adapters;

public class SearchBooksAdapter(LibraryManagementDbContext context, IBookEntityMapper mapper) : ISearchBooksPort
{
    public async Task<SearchResult<Book>> Search(string? searchTerm, Pagination pagination)
    {
        Expression<Func<BookEntity, bool>> filter = b => EF.Functions.ILike(b.Title, $"%{searchTerm}%");

        var count = context.Books.LongCount(filter);

        var books = await context.Books
            .Include(b => b.Keywords)
            .Where(filter)
            .Skip(pagination.PageIndex * pagination.PageSize)
            .Take(pagination.PageSize).ToListAsync();

        return new()
        {
            Results = books.Select(mapper.ToDomain),
            Pagination = new()
            {
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize,
                TotalItems = count
            }
        };
    }
}
