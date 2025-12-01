using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Persistence.Mongo.Domains.Books;
using LibraryManagement.Persistence.Mongo.Domains.Books.Adapters;
using LibraryManagement.Persistence.Mongo.Tests.Infrastructure;

using MongoDB.Driver;

using Moq;

namespace LibraryManagement.Persistence.Mongo.Tests.Domains.Books;

public class SearchBooksAdapterTests
{
    [Fact]
    public async Task Search_with_term_returns_titles_containing_term()
    {
        List<BookEntity> seededBooks = new()
        {
            new BookEntity
            {
                Title = "Clean Code", AuthorId = "author-1", Description = "Code craftsmanship",
                Keywords = new List<string> { "clean-code" }
            },
            new BookEntity
            {
                Title = "Domain-Driven Design", AuthorId = "author-2", Description = "DDD fundamentals",
                Keywords = new List<string> { "ddd" }
            },
            new BookEntity
            {
                Title = "Code Complete", AuthorId = "author-3", Description = "Complete guide",
                Keywords = new List<string> { "code" }
            }
        };

        SearchBooksAdapter adapter = BuildAdapter(seededBooks, out _);

        var results = (await adapter.Search("Clean", new Pagination(0, 10))).ToList();

        Assert.Single(results);
        Assert.Equal("Clean Code", results[0].Title);
    }

    [Fact]
    public async Task Search_without_term_limits_to_ten_results()
    {
        var seededBooks = Enumerable
            .Range(1, 12)
            .Select(index => new BookEntity
            {
                Title = $"Book {index:00}",
                AuthorId = $"author-{index:00}",
                Description = $"Description {index:00}",
                Keywords = new List<string> { $"kw-{index:00}" }
            })
            .ToList();

        FindOptions<BookEntity, BookEntity>? usedOptions = null;
        SearchBooksAdapter adapter = BuildAdapter(seededBooks, out _, options => usedOptions = options);

        var results = (await adapter.Search(null, new Pagination(0, 10))).ToList();

        Assert.Equal(10, results.Count);
        Assert.Equal(10, usedOptions?.Limit);
        Assert.Equal(0, usedOptions?.Skip);
    }

    private static SearchBooksAdapter BuildAdapter(
        List<BookEntity> seededBooks,
        out Mock<IMongoCollection<BookEntity>> collectionMock,
        Action<FindOptions<BookEntity, BookEntity>?>? optionsCallback = null)
    {
        collectionMock = new Mock<IMongoCollection<BookEntity>>();

        collectionMock
            .Setup(collection => collection.FindAsync(
                It.IsAny<FilterDefinition<BookEntity>>(),
                It.IsAny<FindOptions<BookEntity, BookEntity>?>(),
                It.IsAny<CancellationToken>()))
            .Returns((FilterDefinition<BookEntity> filter, FindOptions<BookEntity, BookEntity>? options,
                CancellationToken _) =>
            {
                optionsCallback?.Invoke(options);

                Func<BookEntity, bool> predicate = ResolvePredicate(filter);
                IEnumerable<BookEntity> filtered = seededBooks.Where(predicate);
                if (options?.Limit is int limit) filtered = filtered.Take(limit);

                return Task.FromResult<IAsyncCursor<BookEntity>>(new AsyncCursorStub<BookEntity>(filtered.ToList()));
            });

        Mock<IBookCollection> bookCollectionMock = new();
        bookCollectionMock.SetupGet(collection => collection.Collection).Returns(collectionMock.Object);

        return new SearchBooksAdapter(bookCollectionMock.Object, new BookEntityMapper());
    }

    private static Func<BookEntity, bool> ResolvePredicate(FilterDefinition<BookEntity> filter)
    {
        if (filter is ExpressionFilterDefinition<BookEntity> expressionFilter)
            return expressionFilter.Expression.Compile();

        return _ => true;
    }
}
