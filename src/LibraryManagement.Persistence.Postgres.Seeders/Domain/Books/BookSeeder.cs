using LibraryManagement.Persistence.Postgres.Domains.Books;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Seeders.Domain.Books;

internal static class BookSeeder
{
    internal static DbContext SeedBooks(this DbContext context)
    {
        var books = context.Set<BookEntity>();

        if (books.Any())
        {
            return context;
        }

        return context;
    }
}
