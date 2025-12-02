using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Persistence.Mongo.Domains.Authors;
using LibraryManagement.Persistence.Mongo.Domains.Authors.Adapters;
using LibraryManagement.Persistence.Mongo.Tests.Infrastructure;

using MongoDB.Driver;

using Moq;

namespace LibraryManagement.Persistence.Mongo.Tests.Domains.Authors;

public class SearchAuthorsAdapterTests
{
    [Fact]
    public async Task Search_with_term_returns_names_containing_term()
    {
        List<AuthorEntity> seededAuthors =
        [
            new() { Name = "Kent Beck" },
            new() { Name = "George Martin" },
            new() { Name = "Martin Fowler" }
        ];

        SearchAuthorsAdapter adapter = BuildAdapter(seededAuthors, out _);

        SearchResult<LibraryManagement.Domain.Domains.Authors.Author> results =
            await adapter.Search("George", new Pagination(0, 10));

        var author = Assert.Single(results.Results);
        Assert.Equal("George Martin", author.Name);
    }

    [Fact]
    public async Task Search_without_term_limits_to_page_size()
    {
        var seededAuthors = Enumerable
            .Range(1, 12)
            .Select(index => new AuthorEntity { Name = $"Author {index:00}" })
            .ToList();

        FindOptions<AuthorEntity, AuthorEntity>? usedOptions = null;
        SearchAuthorsAdapter adapter = BuildAdapter(seededAuthors, out _, options => usedOptions = options);

        SearchResult<LibraryManagement.Domain.Domains.Authors.Author> results =
            await adapter.Search(null, new Pagination(0, 10));

        Assert.Equal(10, results.Results.Count());
        Assert.Equal(10, usedOptions?.Limit);
        Assert.Equal(0, usedOptions?.Skip);
    }

    private static SearchAuthorsAdapter BuildAdapter(
        List<AuthorEntity> seededAuthors,
        out Mock<IMongoCollection<AuthorEntity>> collectionMock,
        Action<FindOptions<AuthorEntity, AuthorEntity>?>? optionsCallback = null)
    {
        collectionMock = new Mock<IMongoCollection<AuthorEntity>>();

        collectionMock
            .Setup(collection => collection.FindAsync(
                It.IsAny<FilterDefinition<AuthorEntity>>(),
                It.IsAny<FindOptions<AuthorEntity, AuthorEntity>?>(),
                It.IsAny<CancellationToken>()))
            .Returns((FilterDefinition<AuthorEntity> filter, FindOptions<AuthorEntity, AuthorEntity>? options,
                CancellationToken _) =>
            {
                optionsCallback?.Invoke(options);

                Func<AuthorEntity, bool> predicate = ResolvePredicate(filter);
                IEnumerable<AuthorEntity> filtered = seededAuthors.Where(predicate);
                if (options?.Skip is { } skip) filtered = filtered.Skip(skip);
                if (options?.Limit is int limit) filtered = filtered.Take(limit);

                return Task.FromResult<IAsyncCursor<AuthorEntity>>(new AsyncCursorStub<AuthorEntity>(filtered.ToList()));
            });

        Mock<IAuthorCollection> authorCollectionMock = new();
        authorCollectionMock.SetupGet(collection => collection.Collection).Returns(collectionMock.Object);

        return new SearchAuthorsAdapter(authorCollectionMock.Object, new AuthorEntityMapper());
    }

    private static Func<AuthorEntity, bool> ResolvePredicate(FilterDefinition<AuthorEntity> filter)
    {
        if (filter is ExpressionFilterDefinition<AuthorEntity> expressionFilter)
            return expressionFilter.Expression.Compile();

        return _ => true;
    }
}
