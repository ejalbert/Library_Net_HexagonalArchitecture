using LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;
using LibraryManagement.Persistence.Postgres.DbContexts.Multitenants.Domains.Tenants;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Seeders.Infrastructure.Tenants;

public static class TenantSeeder
{
    extension<TDbContext>(TDbContext context) where TDbContext : DbContext
    {
        public TDbContext SeedTenants(IGetCurrentUserTenantIdUseCase getCurrentUserTenantIdUseCase)
        {
            DbSet<TenantEntity> tenants = context.Set<TenantEntity>();

            if (tenants.Any())
            {
                Console.WriteLine("Books already seeded. Skipping book seeding.");
                return context;
            }

            var tenant = new TenantEntity
            {
                Id = Guid.Parse(getCurrentUserTenantIdUseCase.GetTenantId(new GetCurrentUserTenantIdCommand())),
                Name = "Tenant 1"
            };

            tenants.Add(tenant);
            context.SaveChanges();

            return context;
        }
    }
}
