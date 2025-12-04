using LibraryManagement.Persistence.Postgres.DbContexts.Multitenants.Domains.Tenants;

namespace LibraryManagement.Persistence.Postgres.DbContexts.Multitenants;

public interface IMultitenantEntity
{
    Guid TenantId { get; set; }

    TenantEntity Tenant { get; set; }
}

public abstract class MultitenantEntityBase : IMultitenantEntity
{
    public Guid TenantId { get; set; }

    public TenantEntity Tenant { get; set; } = null!;
}
