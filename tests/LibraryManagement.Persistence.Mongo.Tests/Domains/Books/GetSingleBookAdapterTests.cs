using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Persistence.Mongo.Domains.Books;
using LibraryManagement.Persistence.Mongo.Domains.Books.Adapters;
using LibraryManagement.Persistence.Mongo.Tests.Infrastructure;

using MongoDB.Driver;

using Moq;

namespace LibraryManagement.Persistence.Mongo.Tests.Domains.Books;

public class GetSingleBookAdapterTests
{
    [Fact]
    public async Task GetById_returns_matching_book()
    {
        BookEntity existing = new()
        {
            Id = "book-1",
            Title = "Clean Code",
            AuthorId = "author-1"
        };

        GetSingleBookAdapter adapter = BuildAdapter(new List<BookEntity> { existing });

        Book result = await adapter.GetById(existing.Id);

        Assert.Equal(existing.Id, result.Id);
        Assert.Equal(existing.Title, result.Title);
        Assert.Equal(existing.AuthorId, result.AuthorId);
    }

    [Fact]
    public async Task GetById_without_match_throws()
    {
        GetSingleBookAdapter adapter = BuildAdapter(new List<BookEntity>());

        await Assert.ThrowsAsync<InvalidOperationException>(() => adapter.GetById("book-missing"));
    }

    private static GetSingleBookAdapter BuildAdapter(IReadOnlyCollection<BookEntity> seededBooks)
    {
        Mock<IMongoCollection<BookEntity>> collectionMock = new();

        collectionMock
            .Setup(collection => collection.FindAsync(
                It.IsAny<FilterDefinition<BookEntity>>(),
                It.IsAny<FindOptions<BookEntity, BookEntity>?>(),
                It.IsAny<CancellationToken>()))
            .Returns((FilterDefinition<BookEntity> filter, FindOptions<BookEntity, BookEntity>? _, CancellationToken _) =>
            {
                Func<BookEntity, bool> predicate = ResolvePredicate(filter);
                IEnumerable<BookEntity> filtered = seededBooks.Where(predicate);
                return Task.FromResult<IAsyncCursor<BookEntity>>(new AsyncCursorStub<BookEntity>(filtered.ToList()));
            });

        Mock<IBookCollection> bookCollectionMock = new();
        bookCollectionMock.SetupGet(collection => collection.Collection).Returns(collectionMock.Object);

        return new GetSingleBookAdapter(bookCollectionMock.Object, new BookEntityMapper());
    }

    private static Func<BookEntity, bool> ResolvePredicate(FilterDefinition<BookEntity> filter)
    {
        if (filter is ExpressionFilterDefinition<BookEntity> expressionFilter)
        {
            return expressionFilter.Expression.Compile();
        }

        return _ => true;
    }
}
