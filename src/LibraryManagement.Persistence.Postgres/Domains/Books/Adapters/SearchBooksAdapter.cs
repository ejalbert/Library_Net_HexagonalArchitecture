using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Search;
using LibraryManagement.Persistence.Postgres.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Domains.Books.Adapters;

public class SearchBooksAdapter(LibraryManagementDbContext context, IBookEntityMapper mapper) : ISearchBooksPort
{
    public async Task<IEnumerable<Book>> Search(string? searchTerm)
    {
        var books = await context.Books
            .Include(b => b.Keywords)
            .Where(b => EF.Functions.ILike(b.Title, $"%{searchTerm}%"))
            .Skip(0)
            .Take(10).ToListAsync();

        return books.Select(mapper.ToDomain);
    }
}
