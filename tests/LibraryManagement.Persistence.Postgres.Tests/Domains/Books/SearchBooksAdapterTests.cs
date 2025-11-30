using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Persistence.Postgres.DbContext;
using LibraryManagement.Persistence.Postgres.Domains.Books;
using LibraryManagement.Persistence.Postgres.Domains.Books.Adapters;
using LibraryManagement.Persistence.Postgres.Tests.Infrastructure;

namespace LibraryManagement.Persistence.Postgres.Tests.Domains.Books;

[Collection(nameof(PostgresDatabaseCollection))]
public class SearchBooksAdapterTests(PostgresDatabaseFixture fixture)
{
    [Fact]
    public async Task Search_with_term_returns_titles_containing_term()
    {
        await fixture.ResetDatabaseAsync();
        await using LibraryManagementDbContext context = fixture.CreateDbContext();

        context.Books.AddRange(
            new BookEntity
            {
                Title = "Clean Code",
                AuthorId = "author-1",
                Description = "Code craftsmanship",
                Keywords = new List<BookKeywordEntity> { new() { Keyword = "clean-code" } }
            },
            new BookEntity
            {
                Title = "Domain-Driven Design",
                AuthorId = "author-2",
                Description = "DDD fundamentals",
                Keywords = new List<BookKeywordEntity> { new() { Keyword = "ddd" } }
            },
            new BookEntity
            {
                Title = "Code Complete",
                AuthorId = "author-3",
                Description = "Complete guide",
                Keywords = new List<BookKeywordEntity> { new() { Keyword = "code" } }
            });
        await context.SaveChangesAsync();

        SearchBooksAdapter adapter = new(context, new BookEntityMapper());

        List<Book> results = (await adapter.Search("Clean")).ToList();

        Assert.Single(results);
        Assert.Equal("Clean Code", results[0].Title);
    }

    [Fact]
    public async Task Search_without_term_limits_to_ten_results()
    {
        await fixture.ResetDatabaseAsync();
        await using LibraryManagementDbContext context = fixture.CreateDbContext();

        var books = Enumerable.Range(1, 12).Select(index => new BookEntity
        {
            Title = $"Book {index:00}",
            AuthorId = $"author-{index:00}",
            Description = $"Description {index:00}",
            Keywords = new List<BookKeywordEntity> { new() { Keyword = $"kw-{index:00}" } }
        });
        context.Books.AddRange(books);
        await context.SaveChangesAsync();

        SearchBooksAdapter adapter = new(context, new BookEntityMapper());

        List<Book> results = (await adapter.Search(null)).ToList();

        Assert.Equal(10, results.Count);
    }
}
