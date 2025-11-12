using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Persistence.Mongo.Domains.Books;
using LibraryManagement.Persistence.Mongo.Domains.Books.Adapters;

using MongoDB.Driver;

using Moq;

namespace LibraryManagement.Persistence.Mongo.Tests.Domains.Books;

public class UpdateBookAdapterTests
{
    [Fact]
    public async Task Update_returns_mapped_entity_when_document_exists()
    {
        BookEntity updatedEntity = new() { Id = "book-42", Title = "Refactoring (Updated)" };
        Mock<IMongoCollection<BookEntity>> collectionMock = new();
        collectionMock
            .Setup(collection => collection.FindOneAndUpdateAsync(
                It.IsAny<FilterDefinition<BookEntity>>(),
                It.IsAny<UpdateDefinition<BookEntity>>(),
                It.IsAny<FindOneAndUpdateOptions<BookEntity, BookEntity>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedEntity);

        Mock<IBookCollection> bookCollectionMock = new();
        bookCollectionMock.SetupGet(collection => collection.Collection).Returns(collectionMock.Object);

        Book mapped = new() { Id = updatedEntity.Id, Title = updatedEntity.Title };
        Mock<IBookEntityMapper> mapperMock = new();
        mapperMock.Setup(mapper => mapper.ToDomain(updatedEntity)).Returns(mapped);

        UpdateBookAdapter adapter = new(bookCollectionMock.Object, mapperMock.Object);

        Book result = await adapter.Update("book-42", "Refactoring (Updated)");

        Assert.Same(mapped, result);

        collectionMock.Verify(collection => collection.FindOneAndUpdateAsync(
                It.IsAny<FilterDefinition<BookEntity>>(),
                It.IsAny<UpdateDefinition<BookEntity>>(),
                It.IsAny<FindOneAndUpdateOptions<BookEntity, BookEntity>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Update_when_document_missing_throws()
    {
        Mock<IMongoCollection<BookEntity>> collectionMock = new();
        collectionMock
            .Setup(collection => collection.FindOneAndUpdateAsync(
                It.IsAny<FilterDefinition<BookEntity>>(),
                It.IsAny<UpdateDefinition<BookEntity>>(),
                It.IsAny<FindOneAndUpdateOptions<BookEntity, BookEntity>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((BookEntity?)null);

        Mock<IBookCollection> bookCollectionMock = new();
        bookCollectionMock.SetupGet(collection => collection.Collection).Returns(collectionMock.Object);

        UpdateBookAdapter adapter = new(bookCollectionMock.Object, Mock.Of<IBookEntityMapper>());

        await Assert.ThrowsAsync<InvalidOperationException>(() => adapter.Update("missing", "anything"));
    }
}
