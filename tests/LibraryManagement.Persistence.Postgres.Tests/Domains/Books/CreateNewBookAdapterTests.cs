using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Persistence.Postgres.Domains.Books;
using LibraryManagement.Persistence.Postgres.Domains.Books.Adapters;
using LibraryManagement.Persistence.Postgres.Tests.Infrastructure;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Tests.Domains.Books;

[Collection(nameof(PostgresDatabaseCollection))]
public class CreateNewBookAdapterTests(PostgresDatabaseFixture fixture)
{
    [Fact]
    public async Task Create_persists_book_with_keywords_and_returns_domain_model()
    {
        await fixture.ResetDatabaseAsync();

        await using LibraryManagementDbContext context = fixture.CreateDbContext();
        BookEntityMapper mapper = new();
        CreateNewBookAdapter adapter = new(mapper, context);

        string title = "Test-Driven Development";
        string authorId = "author-123";
        string description = "How to drive design with tests";
        IReadOnlyCollection<string> keywords = new[] { "tdd", "red-green-refactor" };

        Book created = await adapter.Create(title, authorId, description, keywords);

        BookEntity persisted = await context.Books.Include(b => b.Keywords).SingleAsync();

        Assert.Equal(persisted.Id.ToString(), created.Id);
        Assert.Equal(title, created.Title);
        Assert.Equal(authorId, created.AuthorId);
        Assert.Equal(description, created.Description);
        Assert.Equal(keywords, created.Keywords);

        Assert.Equal(title, persisted.Title);
        Assert.Equal(authorId, persisted.AuthorId);
        Assert.Equal(description, persisted.Description);
        Assert.Equal(keywords.Count, persisted.Keywords.Count);
        Assert.All(persisted.Keywords, k => Assert.Equal(persisted.Id, k.BookId));
    }
}
