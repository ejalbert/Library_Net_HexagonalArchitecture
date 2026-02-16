using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Postgres.DbContexts.Multitenants.Domains.Tenants;

internal static class ConfigureTenantDbSet
{
    extension(ModelBuilder modelBuilder)
    {
        internal ModelBuilder ConfigureTenants()
        {
            EntityTypeBuilder<TenantEntity> typeBuilder = modelBuilder.Entity<TenantEntity>();

            typeBuilder.ToTable("Tenants").HasKey(t => t.Id);

            return modelBuilder;
        }
    }
}
