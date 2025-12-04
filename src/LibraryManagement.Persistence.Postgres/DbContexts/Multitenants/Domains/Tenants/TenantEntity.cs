namespace LibraryManagement.Persistence.Postgres.DbContexts.Multitenants.Domains.Tenants;

public class TenantEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}
