using LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;

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
        var getCurrentUserTenantIdUseCaseMock1 = new Mock<IGetCurrentUserTenantIdUseCase>();
        getCurrentUserTenantIdUseCaseMock1.Setup(x => x.GetTenantId(It.IsAny<GetCurrentUserTenantIdCommand>())).Returns("00000000-0000-0000-0000-000000000001");
        await using (var context1 = fixture.CreateDbContext(getCurrentUserTenantIdUseCaseMock1.Object))
        {
            var bookForCurrentTenant = new LibraryManagement.Persistence.Postgres.Domains.Books.BookEntity
            {
                Title = "Tenant Book",
                Description = "Should be visible",
                Author = new LibraryManagement.Persistence.Postgres.Domains.Authors.AuthorEntity { Name = "Author 1" }
            };
            context1.Books.Add(bookForCurrentTenant);
            await context1.SaveChangesAsync();
        }

        // Insert for other tenant
        var getCurrentUserTenantIdUseCaseMock2 = new Mock<IGetCurrentUserTenantIdUseCase>();
        getCurrentUserTenantIdUseCaseMock2.Setup(x => x.GetTenantId(It.IsAny<GetCurrentUserTenantIdCommand>())).Returns("00000000-0000-0000-0000-000000000002");
        await using (var context2 = fixture.CreateDbContext(getCurrentUserTenantIdUseCaseMock2.Object))
        {
            var bookForOtherTenant = new LibraryManagement.Persistence.Postgres.Domains.Books.BookEntity
            {
                Title = "Other Tenant Book",
                Description = "Should NOT be visible",
                Author = new LibraryManagement.Persistence.Postgres.Domains.Authors.AuthorEntity { Name = "Author 2" }
            };
            context2.Books.Add(bookForOtherTenant);
            await context2.SaveChangesAsync();
        }

        // Query with current tenant context
        await using (var context = fixture.CreateDbContext(getCurrentUserTenantIdUseCaseMock1.Object))
        {
            var books = await context.Books.ToListAsync();
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
        tenant1Mock.Setup(x => x.GetTenantId(It.IsAny<GetCurrentUserTenantIdCommand>())).Returns("00000000-0000-0000-0000-000000000001");
        Guid tenant1Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
        Guid tenant2Id = Guid.Parse("00000000-0000-0000-0000-000000000002");
        Guid bookId;
        await using (var context1 = fixture.CreateDbContext(tenant1Mock.Object))
        {
            var book = new LibraryManagement.Persistence.Postgres.Domains.Books.BookEntity
            {
                Title = "Tenant1 Book",
                Description = "Original",
                Author = new LibraryManagement.Persistence.Postgres.Domains.Authors.AuthorEntity { Name = "Author 1" }
            };
            context1.Books.Add(book);
            await context1.SaveChangesAsync();
            bookId = book.Id;
        }

        // Try to update from tenant 2 context
        var tenant2Mock = new Mock<IGetCurrentUserTenantIdUseCase>();
        tenant2Mock.Setup(x => x.GetTenantId(It.IsAny<GetCurrentUserTenantIdCommand>())).Returns("00000000-0000-0000-0000-000000000002");
        await using (var context2 = fixture.CreateDbContext(tenant2Mock.Object))
        {
            var book = await context2.Books.FindAsync(bookId);
            Assert.Null(book); // Should not be able to access tenant1's book
        }
    }

    [Fact]
    public async Task Cannot_insert_entity_with_different_tenant_id()
    {
        await fixture.ResetDatabaseAsync();

        var tenant1Mock = new Mock<IGetCurrentUserTenantIdUseCase>();
        tenant1Mock.Setup(x => x.GetTenantId(It.IsAny<GetCurrentUserTenantIdCommand>())).Returns("00000000-0000-0000-0000-000000000001");
        Guid tenant1Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
        Guid fakeTenantId = Guid.Parse("00000000-0000-0000-0000-000000000999");

        await using (var context = fixture.CreateDbContext(tenant1Mock.Object))
        {
            var book = new LibraryManagement.Persistence.Postgres.Domains.Books.BookEntity
            {
                Title = "Fake Tenant Book",
                Description = "Should be overridden",
                Author = new LibraryManagement.Persistence.Postgres.Domains.Authors.AuthorEntity { Name = "Author" },
                TenantId = fakeTenantId // Try to set a different tenant
            };
            context.Books.Add(book);
            await context.SaveChangesAsync();

            // Reload and check tenant
            var loaded = await context.Books.FindAsync(book.Id);
            Assert.NotNull(loaded);
            Assert.Equal(tenant1Id, loaded.TenantId); // Should be overridden by interceptor
        }
    }

}
