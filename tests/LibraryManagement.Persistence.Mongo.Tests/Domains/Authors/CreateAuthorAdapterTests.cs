using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Persistence.Mongo.Domains.Authors;
using LibraryManagement.Persistence.Mongo.Domains.Authors.Adapters;

using MongoDB.Driver;

using Moq;

namespace LibraryManagement.Persistence.Mongo.Tests.Domains.Authors;

public class CreateAuthorAdapterTests
{
    [Fact]
    public async Task Create_persists_author_and_returns_domain_author()
    {
        Mock<IMongoCollection<AuthorEntity>> collectionMock = new();
        AuthorEntity? inserted = null;

        collectionMock
            .Setup(collection => collection.InsertOneAsync(
                It.IsAny<AuthorEntity>(),
                It.IsAny<InsertOneOptions?>(),
                It.IsAny<CancellationToken>()))
            .Callback<AuthorEntity, InsertOneOptions?, CancellationToken>((entity, _, _) => inserted = entity)
            .Returns(Task.CompletedTask);

        Mock<IAuthorCollection> authorCollectionMock = new();
        authorCollectionMock.SetupGet(collection => collection.Collection).Returns(collectionMock.Object);

        CreateAuthorAdapter adapter = new(authorCollectionMock.Object, new AuthorEntityMapper());

        Author result = await adapter.Create("Robert C. Martin");

        Assert.NotNull(inserted);
        Assert.Equal("Robert C. Martin", inserted!.Name);
        Assert.Equal(inserted.Id, result.Id);
        Assert.Equal(inserted.Name, result.Name);

        collectionMock.Verify(collection => collection.InsertOneAsync(
                It.IsAny<AuthorEntity>(),
                It.IsAny<InsertOneOptions?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
