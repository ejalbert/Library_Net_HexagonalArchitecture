using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Tests.Infrastructure;

[CollectionDefinition(nameof(PostgresDatabaseCollection))]
public sealed class PostgresDatabaseCollection : ICollectionFixture<PostgresDatabaseFixture>;

public sealed class PostgresDatabaseFixture : IAsyncLifetime
{
    private readonly IContainer _postgresContainer;

    public PostgresDatabaseFixture()
    {
        _postgresContainer = new ContainerBuilder()
            .WithImage("postgres:18.1")
            .WithEnvironment("POSTGRES_USER", "postgres")
            .WithEnvironment("POSTGRES_PASSWORD", "postgres")
            .WithEnvironment("POSTGRES_DB", "library_test")
            .WithPortBinding(0, 5432)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilMessageIsLogged("database system is ready to accept connections"))
            .Build();
    }

    public string ConnectionString { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();

        int mappedPort = _postgresContainer.GetMappedPublicPort(5432);
        string host = _postgresContainer.Hostname;

        ConnectionString =
            $"Host={host};Port={mappedPort};Database=library_test;Username=postgres;Password=postgres";

        await ResetDatabaseAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
    }

    public LibraryManagementDbContext CreateDbContext()
    {
        DbContextOptions<LibraryManagementDbContext> options =
            new DbContextOptionsBuilder<LibraryManagementDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;

        return new LibraryManagementDbContext(options);
    }

    public async Task ResetDatabaseAsync()
    {
        await using LibraryManagementDbContext context = CreateDbContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
}
