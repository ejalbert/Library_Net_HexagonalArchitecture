using LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;
using LibraryManagement.Persistence.Postgres.DbContexts;

using Moq;

namespace LibraryManagement.Persistence.Postgres.Tests.Infrastructure.Multitenancy;

[Collection(nameof(PostgresDatabaseCollection))]
public class MultitenancyAcceptanceTests(PostgresDatabaseFixture fixture)
{
    [Fact]
    public async Task Multitenancy_is_enforced_on_entities()
    {
        await fixture.ResetDatabaseAsync();

        var getCurrentUserTenantIdUseCaseMock = new Mock<IGetCurrentUserTenantIdUseCase>();

        getCurrentUserTenantIdUseCaseMock.Setup(x=>x.GetTenantId(It.IsAny<GetCurrentUserTenantIdCommand>())).Returns("00000000-0000-0000-0000-000000000001");

        await using LibraryManagementDbContext context = fixture.CreateDbContext(getCurrentUserTenantIdUseCaseMock.Object);



    }

}
