using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Persistence.Postgres.DbContexts;
using LibraryManagement.Persistence.Postgres.Domains.Authors;
using LibraryManagement.Persistence.Postgres.Domains.Authors.Adapters;
using LibraryManagement.Persistence.Postgres.Seeders.Domain.Authors;
using LibraryManagement.Persistence.Postgres.Tests.Infrastructure;

namespace LibraryManagement.Persistence.Postgres.Tests.Domains.Authors;

[Collection(nameof(PostgresDatabaseCollection))]
public class SearchAuthorsAdapterTests(PostgresDatabaseFixture fixture)
{
    [Fact]
    public async Task Search_with_term_filters_by_name()
    {
        await fixture.ResetDatabaseAsync();
        await using LibraryManagementDbContext context = fixture.CreateDbContext();
        context.SeedAuthors();

        SearchAuthorsAdapter adapter = new(context, new AuthorEntityMapper());

        SearchResult<Author> results = await adapter.Search("Rowling", new Pagination(0, 10));

        Author author = Assert.Single(results.Results);
        Assert.Equal("J.K. Rowling", author.Name);
    }

    [Fact]
    public async Task Search_without_term_limits_to_page_size()
    {
        await fixture.ResetDatabaseAsync();
        await using LibraryManagementDbContext context = fixture.CreateDbContext();

        IEnumerable<AuthorEntity> authors = Enumerable.Range(1, 12)
            .Select(index => new AuthorEntity { Name = $"Author {index:00}" });
        context.Authors.AddRange(authors);
        await context.SaveChangesAsync();

        SearchAuthorsAdapter adapter = new(context, new AuthorEntityMapper());

        SearchResult<Author> results = await adapter.Search(null, new Pagination(0, 10));

        Assert.Equal(10, results.Results.Count());
        Assert.Equal(12, results.Pagination.TotalItems);
    }
}
