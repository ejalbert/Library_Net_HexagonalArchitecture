namespace LibraryManagement.Persistence.Postgres.DbContexts.Multitenants;

public interface IMultitenantEntity
{
    Guid TenantId { get; set; }
}

public abstract class MultitenantEntityBase : IMultitenantEntity
{
    public Guid TenantId { get; set; }
}
