using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Persistence.Postgres.DbContexts;
using LibraryManagement.Persistence.Postgres.Domains.Books;
using LibraryManagement.Persistence.Postgres.Domains.Books.Adapters;
using LibraryManagement.Persistence.Postgres.Seeders.Domain.Authors;
using LibraryManagement.Persistence.Postgres.Tests.Infrastructure;
using LibraryManagement.Persistence.Postgres.Tests.Domains.Authors.Extensions;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Tests.Domains.Books;

[Collection(nameof(PostgresDatabaseCollection))]
public class UpdateBookAdapterTests(PostgresDatabaseFixture fixture)
{
    [Fact]
    public async Task Update_overwrites_all_fields_and_keywords()
    {
        await fixture.ResetDatabaseAsync();


        await using LibraryManagementDbContext context = fixture.CreateDbContext().SeedAuthors();

        var authorId = context.Authors.JkRowling.Id;

        BookEntity entity = new()
        {
            Title = "Original Title",
            AuthorId = authorId,
            Description = "Original description",
            Keywords = [new BookKeywordEntity { Keyword = "legacy" }]
        };

        context.Books.Add(entity);
        await context.SaveChangesAsync();

        UpdateBookAdapter adapter = new(new BookEntityMapper(), context);

        Book updated = await adapter.Update(entity.Id.ToString(), "Updated Title", authorId.ToString(),
            "Updated description", new[] { "fresh", "updated" });

        BookEntity persisted = await context.Books.Include(b => b.Keywords).SingleAsync();

        Assert.Equal(entity.Id, persisted.Id);
        Assert.Equal("Updated Title", persisted.Title);
        Assert.Equal(authorId, persisted.AuthorId);
        Assert.Equal("Updated description", persisted.Description);
        Assert.Equal(new[] { "fresh", "updated" }, persisted.Keywords.Select(k => k.Keyword));

        Assert.Equal(persisted.Id.ToString(), updated.Id);
        Assert.Equal(persisted.Title, updated.Title);
        Assert.Equal(persisted.AuthorId.ToString(), updated.AuthorId);
        Assert.Equal(persisted.Description, updated.Description);
        Assert.Equal(persisted.Keywords.Select(k => k.Keyword), updated.Keywords);
    }

    [Fact]
    public async Task Update_unknown_book_throws()
    {
        await fixture.ResetDatabaseAsync();

        await using LibraryManagementDbContext context = fixture.CreateDbContext();

        UpdateBookAdapter adapter = new(new BookEntityMapper(), context);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            adapter.Update(Guid.NewGuid().ToString(), "title", "author", "desc", Array.Empty<string>()));
    }
}
