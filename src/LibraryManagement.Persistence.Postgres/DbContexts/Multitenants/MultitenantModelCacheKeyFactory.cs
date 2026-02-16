using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LibraryManagement.Persistence.Postgres.DbContexts.Multitenants;

internal class MultitenantModelCacheKeyFactory : IModelCacheKeyFactory
{
    public object Create(DbContext context, bool designTime)
    {
        if (context is MultitenantDbContext multitenantDbContext)
            return (context.GetType(), multitenantDbContext.CurrentTenantId, designTime);

        return (context.GetType(), designTime);
    }
}
