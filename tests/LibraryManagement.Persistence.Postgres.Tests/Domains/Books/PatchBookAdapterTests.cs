using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Persistence.Postgres.DbContexts;
using LibraryManagement.Persistence.Postgres.Domains.Books;
using LibraryManagement.Persistence.Postgres.Domains.Books.Adapters;
using LibraryManagement.Persistence.Postgres.Tests.Infrastructure;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Tests.Domains.Books;

[Collection(nameof(PostgresDatabaseCollection))]
public class PatchBookAdapterTests(PostgresDatabaseFixture fixture)
{
    [Fact]
    public async Task Patch_updates_specified_fields_and_keywords()
    {
        await fixture.ResetDatabaseAsync();

        var authorId = DbContextSeeder.Authors.AuthorOne.Id;

        await using LibraryManagementDbContext context = fixture.CreateDbContext().WithAuthors();
        BookEntity entity = new()
        {
            Title = "Existing Title",
            AuthorId = authorId,
            Description = "Existing description",
            Keywords =
            [
                new BookKeywordEntity { Keyword = "initial" },
                new BookKeywordEntity { Keyword = "value" }
            ]
        };

        context.Books.Add(entity);
        await context.SaveChangesAsync();

        PatchBookAdapter adapter = new(new BookEntityMapper(), context);

        Book patched = await adapter.Patch(entity.Id.ToString(), null, authorId.ToString(), null, new[] { "patched" });

        BookEntity persisted = await context.Books.Include(b => b.Keywords).SingleAsync();

        Assert.Equal("Existing Title", persisted.Title);
        Assert.Equal(authorId, persisted.AuthorId);
        Assert.Equal("Existing description", persisted.Description);
        Assert.Equal(new[] { "patched" }, persisted.Keywords.Select(k => k.Keyword));

        Assert.Equal(persisted.Id.ToString(), patched.Id);
        Assert.Equal(persisted.Title, patched.Title);
        Assert.Equal(persisted.AuthorId.ToString(), patched.AuthorId);
        Assert.Equal(persisted.Description, patched.Description);
        Assert.Equal(persisted.Keywords.Select(k => k.Keyword), patched.Keywords);
    }

    [Fact]
    public async Task Patch_unknown_book_throws()
    {
        await fixture.ResetDatabaseAsync();

        await using LibraryManagementDbContext context = fixture.CreateDbContext();

        PatchBookAdapter adapter = new(new BookEntityMapper(), context);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            adapter.Patch(Guid.NewGuid().ToString(), "title", null, null, null));
    }
}
