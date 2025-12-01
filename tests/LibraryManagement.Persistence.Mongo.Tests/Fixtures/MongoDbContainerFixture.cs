using LibraryManagement.Tests.Abstractions;

using MongoDB.Driver;

using Testcontainers.MongoDb;

namespace LibraryManagement.Persistence.Mongo.Tests.Fixtures;

[CollectionDefinition(nameof(MongoDbCollection))]
public class MongoDbCollection : ICollectionFixture<MongoDbContainerFixture>;

public sealed class MongoDbContainerFixture : IAsyncLifetime
{
    private readonly MongoDbContainer _container;

    public MongoDbContainerFixture()
    {
        DockerApiCompatibility.EnsureDockerApiVersion();

        _container = new MongoDbBuilder()
            .WithImage("mongo:7.0")
            .WithCleanUp(true)
            .Build();
    }

    private IMongoClient? _client;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        _client = new MongoClient(_container.GetConnectionString());
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    public IMongoDatabase CreateCleanDatabase()
    {
        if (_client is null) throw new InvalidOperationException("Mongo client is not initialized yet.");

        return _client.GetDatabase($"library_management_tests_{Guid.NewGuid():N}");
    }

    public Task DropDatabaseAsync(IMongoDatabase database)
    {
        if (_client is null) return Task.CompletedTask;

        return _client.DropDatabaseAsync(database.DatabaseNamespace.DatabaseName);
    }
}
