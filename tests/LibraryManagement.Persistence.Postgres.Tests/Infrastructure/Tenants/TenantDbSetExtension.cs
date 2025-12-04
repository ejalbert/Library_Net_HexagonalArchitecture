using LibraryManagement.Persistence.Postgres.DbContexts.Multitenants.Domains.Tenants;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Tests.Infrastructure.Tenants;

internal static class TenantDbSetExtension
{
    extension(DbSet<TenantEntity> tenants)
    {
        internal TenantEntity Tenant1 => tenants.Single(t => t.Name == "Tenant 1");
    }
}
