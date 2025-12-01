using LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;
using LibraryManagement.Persistence.Postgres.DbContexts;
using Microsoft.EntityFrameworkCore;
using Xunit;

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

}
