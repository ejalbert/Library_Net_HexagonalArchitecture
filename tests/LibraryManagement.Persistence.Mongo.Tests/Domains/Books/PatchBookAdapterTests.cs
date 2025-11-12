using System.Collections.Generic;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Patch;
using LibraryManagement.Persistence.Mongo.Domains.Books;
using LibraryManagement.Persistence.Mongo.Domains.Books.Adapters;
using MongoDB.Driver;
using Moq;

namespace LibraryManagement.Persistence.Mongo.Tests.Domains.Books;

public class PatchBookAdapterTests
{
    [Fact]
    public async Task Patch_updates_only_specified_fields()
    {
        BookEntity updatedEntity = new()
        {
            Id = "book-1",
            Title = "Clean Code",
            AuthorId = "author-2",
            Description = "New description",
            Keywords = new List<string> { "clean-code" }
        };
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

        Book mapped = new()
        {
            Id = updatedEntity.Id,
            Title = updatedEntity.Title,
            AuthorId = updatedEntity.AuthorId,
            Description = updatedEntity.Description,
            Keywords = updatedEntity.Keywords
        };
        Mock<IBookEntityMapper> mapperMock = new();
        mapperMock.Setup(mapper => mapper.ToDomain(updatedEntity)).Returns(mapped);

        PatchBookAdapter adapter = new(bookCollectionMock.Object, mapperMock.Object);

        Book result = await adapter.Patch("book-1", null, "author-2", "New description", new[] { "clean-code" });

        Assert.Same(mapped, result);
        collectionMock.Verify(collection => collection.FindOneAndUpdateAsync(
                It.IsAny<FilterDefinition<BookEntity>>(),
                It.IsAny<UpdateDefinition<BookEntity>>(),
                It.IsAny<FindOneAndUpdateOptions<BookEntity, BookEntity>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
