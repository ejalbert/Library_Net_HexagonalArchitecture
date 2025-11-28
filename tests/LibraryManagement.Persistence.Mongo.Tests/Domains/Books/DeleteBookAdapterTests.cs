using LibraryManagement.Persistence.Mongo.Domains.Books;
using LibraryManagement.Persistence.Mongo.Domains.Books.Adapters;

using MongoDB.Driver;

using Moq;

namespace LibraryManagement.Persistence.Mongo.Tests.Domains.Books;

public class DeleteBookAdapterTests
{
    [Fact]
    public async Task Delete_removes_matching_entity()
    {
        Mock<IMongoCollection<BookEntity>> collectionMock = new();
        collectionMock
            .Setup(collection => collection.DeleteOneAsync(
                It.IsAny<FilterDefinition<BookEntity>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeleteResult.Acknowledged(1));

        Mock<IBookCollection> bookCollectionMock = new();
        bookCollectionMock.SetupGet(collection => collection.Collection).Returns(collectionMock.Object);

        DeleteBookAdapter adapter = new(bookCollectionMock.Object);

        await adapter.Delete("book-123");

        collectionMock.Verify(collection => collection.DeleteOneAsync(
                It.IsAny<FilterDefinition<BookEntity>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Delete_when_no_documents_removed_throws()
    {
        Mock<IMongoCollection<BookEntity>> collectionMock = new();
        collectionMock
            .Setup(collection => collection.DeleteOneAsync(
                It.IsAny<FilterDefinition<BookEntity>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeleteResult.Acknowledged(0));

        Mock<IBookCollection> bookCollectionMock = new();
        bookCollectionMock.SetupGet(collection => collection.Collection).Returns(collectionMock.Object);

        DeleteBookAdapter adapter = new(bookCollectionMock.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() => adapter.Delete("book-missing"));
    }
}
