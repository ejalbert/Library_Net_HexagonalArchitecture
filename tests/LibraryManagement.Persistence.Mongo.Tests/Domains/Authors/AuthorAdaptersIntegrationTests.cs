using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Persistence.Mongo.Domains.Authors;
using LibraryManagement.Persistence.Mongo.Domains.Authors.Adapters;
using LibraryManagement.Persistence.Mongo.Tests.Fixtures;

using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.Tests.Domains.Authors;

[Collection(nameof(MongoDbCollection))]
public class AuthorAdaptersIntegrationTests(MongoDbContainerFixture fixture) : IAsyncLifetime
{
    private readonly AuthorEntityMapper _mapper = new();
    private IAuthorCollection? _authorCollection;
    private IMongoDatabase? _database;

    public Task InitializeAsync()
    {
        _database = fixture.CreateCleanDatabase();
        _authorCollection = new AuthorCollection(_database);
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        if (_database is null) return;

        await fixture.DropDatabaseAsync(_database);
    }

    [Fact]
    public async Task CreateAuthorAdapter_persists_and_returns_domain_author()
    {
        CreateAuthorAdapter adapter = new(GetAuthorCollection(), _mapper);

        Author created = await adapter.Create("Uncle Bob");

        AuthorEntity persisted = await GetAuthorCollection().Collection
            .Find(entity => entity.Id == created.Id)
            .SingleAsync();

        Assert.Equal("Uncle Bob", persisted.Name);
        Assert.Equal(persisted.Id, created.Id);
    }

    private IAuthorCollection GetAuthorCollection()
    {
        return _authorCollection ?? throw new InvalidOperationException("Author collection was not initialized.");
    }
}
