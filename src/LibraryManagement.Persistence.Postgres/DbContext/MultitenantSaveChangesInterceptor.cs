using LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LibraryManagement.Persistence.Postgres.DbContext;

public interface IMultitenantSaveChangesInterceptor : ISaveChangesInterceptor;

public class MultitenantSaveChangesInterceptor(IGetCurrentUserTenantIdUseCase getCurrentUserTenantIdUseCase) : SaveChangesInterceptor, IMultitenantSaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyTenant(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        ApplyTenant(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyTenant(Microsoft.EntityFrameworkCore.DbContext? context)
    {
        if (context is null) return;

        var tenantId = Guid.Parse(getCurrentUserTenantIdUseCase.GetTenantId(new ()));

        foreach (var entry in context.ChangeTracker.Entries<IMultitenantEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    // Always enforce current tenant on new rows
                    entry.Entity.TenantId = tenantId;
                    break;

                case EntityState.Modified:
                case EntityState.Deleted:
                    // Optional: forbid cross-tenant tampering
                    if (entry.Entity.TenantId != tenantId)
                    {
                        throw new InvalidOperationException(
                            $"Cross-tenant modification detected for {entry.Metadata.Name}.");
                    }
                    break;
            }
        }
    }

}
