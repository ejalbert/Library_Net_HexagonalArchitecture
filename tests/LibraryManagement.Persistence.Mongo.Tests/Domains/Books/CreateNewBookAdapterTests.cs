using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Persistence.Mongo.Domains.Books;
using LibraryManagement.Persistence.Mongo.Domains.Books.Adapters;

using MongoDB.Driver;

using Moq;

namespace LibraryManagement.Persistence.Mongo.Tests.Domains.Books;

public class CreateNewBookAdapterTests
{
    [Fact]
    public async Task Create_persists_book_and_returns_domain_book()
    {
        Mock<IMongoCollection<BookEntity>> collectionMock = new();
        BookEntity? insertedEntity = null;

        collectionMock
            .Setup(collection => collection.InsertOneAsync(
                It.IsAny<BookEntity>(),
                It.IsAny<InsertOneOptions?>(),
                It.IsAny<CancellationToken>()))
            .Callback<BookEntity, InsertOneOptions?, CancellationToken>((entity, _, _) => insertedEntity = entity)
            .Returns(Task.CompletedTask);

        Mock<IBookCollection> bookCollectionMock = new();
        bookCollectionMock.SetupGet(collection => collection.Collection).Returns(collectionMock.Object);

        CreateNewBookAdapter adapter = new(bookCollectionMock.Object, new BookEntityMapper());

        Book result = await adapter.Create("Refactoring");

        Assert.NotNull(insertedEntity);
        Assert.Equal("Refactoring", insertedEntity!.Title);
        Assert.Equal(insertedEntity.Id, result.Id);
        Assert.Equal(insertedEntity.Title, result.Title);

        collectionMock.Verify(collection => collection.InsertOneAsync(
                It.IsAny<BookEntity>(),
                It.IsAny<InsertOneOptions?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
