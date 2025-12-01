using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;
using LibraryManagement.Persistence.Postgres.DbContexts;
using LibraryManagement.Persistence.Postgres.DbContexts.Multitenants;
using LibraryManagement.Persistence.Postgres.Domains.Authors;
using LibraryManagement.Tests.Abstractions;

using Microsoft.EntityFrameworkCore;

using Moq;

namespace LibraryManagement.Persistence.Postgres.Tests.Infrastructure;

[CollectionDefinition(nameof(PostgresDatabaseCollection))]
public sealed class PostgresDatabaseCollection : ICollectionFixture<PostgresDatabaseFixture>;

public sealed class PostgresDatabaseFixture : IAsyncLifetime
{
    private readonly IContainer _postgresContainer;

    public PostgresDatabaseFixture()
    {
        DockerApiCompatibility.EnsureDockerApiVersion();

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

    public LibraryManagementDbContext CreateDbContext(IGetCurrentUserTenantIdUseCase? getCurrentUserTenantIdUseCase = null)
    {
        if (getCurrentUserTenantIdUseCase == null)
        {
            var useCaseMock = new Mock<IGetCurrentUserTenantIdUseCase>();
            useCaseMock.Setup(uc => uc.GetTenantId(It.IsAny<GetCurrentUserTenantIdCommand>())).Returns("11111111-2222-3333-4444-555555555555");
            getCurrentUserTenantIdUseCase = useCaseMock.Object;
        }

        DbContextOptions<LibraryManagementDbContext> options =
            new DbContextOptionsBuilder<LibraryManagementDbContext>()
                .UseNpgsql(ConnectionString)
                .AddInterceptors(new MultitenantSaveChangesInterceptor(getCurrentUserTenantIdUseCase))
                .Options;

        var context = new LibraryManagementDbContext(options, getCurrentUserTenantIdUseCase);

        return context ;
    }

    public async Task ResetDatabaseAsync()
    {
        await using LibraryManagementDbContext context = CreateDbContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
}

internal static class DbContextSeeder
{


    extension(LibraryManagementDbContext context)
    {
        internal LibraryManagementDbContext WithAuthors()
        {
            context.Authors.AddRange(
            Authors.AuthorOne,
            Authors.AuthorTwo);

            context.SaveChanges();

            return context;
        }
    }

    internal static class Authors
    {
        internal static AuthorEntity AuthorOne => new()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-111111111111"),
            Name = "Author One"
        };

        internal static AuthorEntity AuthorTwo => new()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-222222222222"),
            Name = "Author Two"
        };
    }
}

