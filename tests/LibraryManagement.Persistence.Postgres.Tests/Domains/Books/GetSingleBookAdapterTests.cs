using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Persistence.Postgres.DbContexts;
using LibraryManagement.Persistence.Postgres.Domains.Books;
using LibraryManagement.Persistence.Postgres.Domains.Books.Adapters;
using LibraryManagement.Persistence.Postgres.Seeders.Domain.Authors;
using LibraryManagement.Persistence.Postgres.Tests.Domains.Authors.Extensions;
using LibraryManagement.Persistence.Postgres.Tests.Infrastructure;

namespace LibraryManagement.Persistence.Postgres.Tests.Domains.Books;

[Collection(nameof(PostgresDatabaseCollection))]
public class GetSingleBookAdapterTests(PostgresDatabaseFixture fixture)
{
    [Fact]
    public async Task GetById_returns_book_with_keywords()
    {
        await fixture.ResetDatabaseAsync();

        await using LibraryManagementDbContext context = fixture.CreateDbContext().SeedAuthors();
        BookEntityMapper mapper = new();
        GetSingleBookAdapter adapter = new(context, mapper);

        BookEntity entity = new()
        {
            Title = "Effective C#",
            AuthorId = context.Authors.JkRowling.Id,
            Description = "Best practices"
        };

        entity.Keywords.Add(new BookKeywordEntity { Book = entity, Keyword = "csharp" });
        entity.Keywords.Add(new BookKeywordEntity { Book = entity, Keyword = "dotnet" });

        context.Books.Add(entity);
        await context.SaveChangesAsync();

        Book result = await adapter.GetById(entity.Id.ToString());

        Assert.Equal(entity.Id.ToString(), result.Id);
        Assert.Equal(entity.Title, result.Title);
        Assert.Equal(entity.AuthorId.ToString(), result.AuthorId);
        Assert.Equal(entity.Description, result.Description);
        Assert.Equal(new[] { "csharp", "dotnet" }, result.Keywords);
    }
}
