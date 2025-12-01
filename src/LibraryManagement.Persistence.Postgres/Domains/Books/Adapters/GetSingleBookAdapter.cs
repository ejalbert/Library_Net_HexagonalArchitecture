using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.GetSingle;
using LibraryManagement.Persistence.Postgres.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Domains.Books.Adapters;

public class GetSingleBookAdapter(LibraryManagementDbContext context, IBookEntityMapper mapper) : IGetSingleBookPort
{
    public async Task<Book> GetById(string id)
    {
        if (!Guid.TryParse(id, out Guid bookId))
        {
            throw new ArgumentException("Invalid book ID format.", nameof(id));
        }

        BookEntity? book = await context.Books
            .Include(entity => entity.Keywords)
            .SingleOrDefaultAsync(entity => entity.Id == bookId);

        if (book == null)
        {
            throw new InvalidOperationException($"Book with ID {id} not found.");
        }

        return mapper.ToDomain(book);
    }
}
