using LibraryManagement.Persistence.Postgres.DbContexts;
using LibraryManagement.Persistence.Postgres.Domains.Books;
using LibraryManagement.Persistence.Postgres.Domains.Books.Adapters;
using LibraryManagement.Persistence.Postgres.Seeders.Domain.Authors;
using LibraryManagement.Persistence.Postgres.Tests.Domains.Authors.Extensions;
using LibraryManagement.Persistence.Postgres.Tests.Infrastructure;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Tests.Domains.Books;

[Collection(nameof(PostgresDatabaseCollection))]
public class DeleteBookAdapterTests(PostgresDatabaseFixture fixture)
{
    [Fact]
    public async Task Delete_existing_book_removes_entity_and_keywords()
    {
        await fixture.ResetDatabaseAsync();

        await using LibraryManagementDbContext context = fixture.CreateDbContext().SeedAuthors();

        var bookId = Guid.NewGuid();
        BookEntity entity = new()
        {
            Id = bookId,
            Title = "Clean Code",
            AuthorId = context.Authors.JkRowling.Id,
            Description = "Craftsmanship",
            Keywords =
            [
                new BookKeywordEntity { BookId = bookId, Keyword = "clean" },
                new BookKeywordEntity { BookId = bookId, Keyword = "code" }
            ]
        };

        context.Books.Add(entity);
        await context.SaveChangesAsync();

        DeleteBookAdapter adapter = new(context);

        await adapter.Delete(bookId.ToString());

        Assert.Empty(await context.Books.ToListAsync());
        Assert.Empty(await context.Set<BookKeywordEntity>().ToListAsync());
    }

    [Fact]
    public async Task Delete_unknown_book_throws()
    {
        await fixture.ResetDatabaseAsync();

        await using LibraryManagementDbContext context = fixture.CreateDbContext();

        DeleteBookAdapter adapter = new(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => adapter.Delete(Guid.NewGuid().ToString()));
    }
}
