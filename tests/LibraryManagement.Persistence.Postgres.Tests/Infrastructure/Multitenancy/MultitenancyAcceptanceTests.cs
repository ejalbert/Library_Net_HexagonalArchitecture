using LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;
using LibraryManagement.Persistence.Postgres.DbContexts;
using LibraryManagement.Persistence.Postgres.DbContexts.Multitenants.Domains.Tenants;
using LibraryManagement.Persistence.Postgres.Domains.Authors;
using LibraryManagement.Persistence.Postgres.Domains.Books;

using Microsoft.EntityFrameworkCore;

using Moq;

namespace LibraryManagement.Persistence.Postgres.Tests.Infrastructure.Multitenancy;

[Collection(nameof(PostgresDatabaseCollection))]
public class MultitenancyAcceptanceTests(PostgresDatabaseFixture fixture)
{
    [Fact]
    public async Task Multitenancy_is_enforced_on_entities()
    {
        await fixture.ResetDatabaseAsync();

        // Insert for current tenant
        var tenant1Guid = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var getCurrentUserTenantIdUseCaseMock1 = new Mock<IGetCurrentUserTenantIdUseCase>();
        getCurrentUserTenantIdUseCaseMock1.Setup(x => x.GetTenantId(It.IsAny<GetCurrentUserTenantIdCommand>()))
            .Returns(tenant1Guid.ToString());
        await using (LibraryManagementDbContext context1 =
                     fixture.CreateDbContext(getCurrentUserTenantIdUseCaseMock1.Object))
        {
            context1.Tenants.Add(new TenantEntity { Id = tenant1Guid, Name = "Tenant 1" });

            var bookForCurrentTenant = new BookEntity
            {
                Title = "Tenant Book",
                Description = "Should be visible",
                Author = new AuthorEntity { Name = "Author 1" }
            };
            context1.Books.Add(bookForCurrentTenant);
            await context1.SaveChangesAsync();
        }

        // Insert for other tenant
        var tenant2Guid = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var getCurrentUserTenantIdUseCaseMock2 = new Mock<IGetCurrentUserTenantIdUseCase>();
        getCurrentUserTenantIdUseCaseMock2.Setup(x => x.GetTenantId(It.IsAny<GetCurrentUserTenantIdCommand>()))
            .Returns(tenant2Guid.ToString);
        await using (LibraryManagementDbContext context2 =
                     fixture.CreateDbContext(getCurrentUserTenantIdUseCaseMock2.Object))
        {
            context2.Tenants.Add(new TenantEntity { Id = tenant2Guid, Name = "Tenant 2" });

            var bookForOtherTenant = new BookEntity
            {
                Title = "Other Tenant Book",
                Description = "Should NOT be visible",
                Author = new AuthorEntity { Name = "Author 2" }
            };
            context2.Books.Add(bookForOtherTenant);
            await context2.SaveChangesAsync();
        }

        // Query with current tenant context
        await using (LibraryManagementDbContext context =
                     fixture.CreateDbContext(getCurrentUserTenantIdUseCaseMock1.Object))
        {
            List<BookEntity> books = await context.Books.ToListAsync();
            Assert.Single(books);
            Assert.Equal("Tenant Book", books[0].Title);
        }
    }

    [Fact]
    public async Task Cannot_update_entity_of_another_tenant()
    {
        await fixture.ResetDatabaseAsync();

        // Insert for tenant 1
        var tenant1Mock = new Mock<IGetCurrentUserTenantIdUseCase>();
        tenant1Mock.Setup(x => x.GetTenantId(It.IsAny<GetCurrentUserTenantIdCommand>()))
            .Returns("00000000-0000-0000-0000-000000000001");
        var tenant1Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var tenant2Id = Guid.Parse("00000000-0000-0000-0000-000000000002");
        Guid bookId;
        await using (LibraryManagementDbContext context1 = fixture.CreateDbContext(tenant1Mock.Object))
        {
            context1.Tenants.AddRange(new TenantEntity { Id = tenant1Id, Name = "Tenant 1" },
                new TenantEntity { Id = tenant2Id, Name = "Tenant 2" });
            var book = new BookEntity
            {
                Title = "Tenant1 Book",
                Description = "Original",
                Author = new AuthorEntity { Name = "Author 1" }
            };
            context1.Books.Add(book);
            await context1.SaveChangesAsync();
            bookId = book.Id;
        }

        // Try to update from tenant 2 context
        var tenant2Mock = new Mock<IGetCurrentUserTenantIdUseCase>();
        tenant2Mock.Setup(x => x.GetTenantId(It.IsAny<GetCurrentUserTenantIdCommand>()))
            .Returns("00000000-0000-0000-0000-000000000002");
        await using (LibraryManagementDbContext context2 = fixture.CreateDbContext(tenant2Mock.Object))
        {
            BookEntity? book = await context2.Books.FindAsync(bookId);
            Assert.Null(book); // Should not be able to access tenant1's book
        }
    }

    [Fact]
    public async Task Cannot_insert_entity_with_different_tenant_id()
    {
        await fixture.ResetDatabaseAsync();

        var tenant1Mock = new Mock<IGetCurrentUserTenantIdUseCase>();
        tenant1Mock.Setup(x => x.GetTenantId(It.IsAny<GetCurrentUserTenantIdCommand>()))
            .Returns("00000000-0000-0000-0000-000000000001");
        var tenant1Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var fakeTenantId = Guid.Parse("00000000-0000-0000-0000-000000000999");

        await using (LibraryManagementDbContext context = fixture.CreateDbContext(tenant1Mock.Object))
        {
            context.Tenants.Add(new TenantEntity { Id = tenant1Id, Name = "Tenant 1" });

            var book = new BookEntity
            {
                Title = "Fake Tenant Book",
                Description = "Should be overridden",
                Author = new AuthorEntity { Name = "Author" },
                TenantId = fakeTenantId // Try to set a different tenant
            };
            context.Books.Add(book);
            await context.SaveChangesAsync();

            // Reload and check tenant
            BookEntity? loaded = await context.Books.FindAsync(book.Id);
            Assert.NotNull(loaded);
            Assert.Equal(tenant1Id, loaded.TenantId); // Should be overridden by interceptor
        }
    }
}
