using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Persistence.Postgres.DbContexts;
using LibraryManagement.Persistence.Postgres.Domains.Books;
using LibraryManagement.Persistence.Postgres.Domains.Books.Adapters;
using LibraryManagement.Persistence.Postgres.Seeders.Domain.Authors;
using LibraryManagement.Persistence.Postgres.Seeders.Domain.Books;
using LibraryManagement.Persistence.Postgres.Tests.Domains.Authors.Extensions;
using LibraryManagement.Persistence.Postgres.Tests.Infrastructure;

namespace LibraryManagement.Persistence.Postgres.Tests.Domains.Books;

[Collection(nameof(PostgresDatabaseCollection))]
public class SearchBooksAdapterTests(PostgresDatabaseFixture fixture)
{
    [Fact]
    public async Task Search_With_Seeded_Books_Returns_Seeded_Books()
    {
        await fixture.ResetDatabaseAsync();
        await using LibraryManagementDbContext context = fixture.CreateDbContext();

        context.SeedAuthors().SeedBooks();


        SearchBooksAdapter adapter = new(context, new BookEntityMapper());



        var results = await adapter.Search("harry", new Pagination(0, 10));

        Assert.Equal(7, results.Results.Count());
    }

    [Fact]
    public async Task Search_with_term_returns_titles_containing_term()
    {
        await fixture.ResetDatabaseAsync();
        await using LibraryManagementDbContext context = fixture.CreateDbContext().SeedAuthors();

        var authorId = context.Authors.JkRowling.Id;

        context.Books.AddRange(
            new BookEntity
            {
                Title = "Clean Code",
                AuthorId = authorId,
                Description = "Code craftsmanship",
                Keywords = new List<BookKeywordEntity> { new() { Keyword = "clean-code" } }
            },
            new BookEntity
            {
                Title = "Domain-Driven Design",
                AuthorId = authorId,
                Description = "DDD fundamentals",
                Keywords = new List<BookKeywordEntity> { new() { Keyword = "ddd" } }
            },
            new BookEntity
            {
                Title = "Code Complete",
                AuthorId = authorId,
                Description = "Complete guide",
                Keywords = new List<BookKeywordEntity> { new() { Keyword = "code" } }
            });
        await context.SaveChangesAsync();

        SearchBooksAdapter adapter = new(context, new BookEntityMapper());

        var results = await adapter.Search("Clean", new Pagination(0, 10));

        Assert.Single(results.Results);
        Assert.Equal("Clean Code", results.Results.First().Title);
    }

    [Fact]
    public async Task Search_without_term_limits_to_ten_results()
    {
        await fixture.ResetDatabaseAsync();
        await using LibraryManagementDbContext context = fixture.CreateDbContext().SeedAuthors();

        var authorId = context.Authors.JkRowling.Id;

        var books = Enumerable.Range(1, 12).Select(index => new BookEntity
        {
            Title = $"Book {index:00}",
            AuthorId = authorId,
            Description = $"Description {index:00}",
            Keywords = new List<BookKeywordEntity> { new() { Keyword = $"kw-{index:00}" } }
        });
        context.Books.AddRange(books);
        await context.SaveChangesAsync();

        SearchBooksAdapter adapter = new(context, new BookEntityMapper());

        var results = await adapter.Search(null, new Pagination(0, 10));

        Assert.Equal(10, results.Results.Count());
    }
}
