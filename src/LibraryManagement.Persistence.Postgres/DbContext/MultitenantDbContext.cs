using System.Linq.Expressions;

using LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;
using LibraryManagement.Persistence.Postgres.Domains.Books;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.DbContext;

public abstract class MultitenantDbContext(DbContextOptions options, IGetCurrentUserTenantIdUseCase getCurrentUserTenantIdUseCase) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var tenantId = Guid.Parse(getCurrentUserTenantIdUseCase.GetTenantId(new()));

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IMultitenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter("Filter by tenant", CreateTenantFilterExpression(entityType.ClrType, tenantId));
            }
        }
        modelBuilder.ConfigureBooks();
    }

    private static LambdaExpression CreateTenantFilterExpression(Type entityType, Guid tenantId)
    {
        var parameter = Expression.Parameter(entityType, "e");
        var tenantIdProperty = Expression.Property(parameter, nameof(IMultitenantEntity.TenantId));
        var currentTenantId = Expression.Constant(tenantId);
        var equality = Expression.Equal(tenantIdProperty, currentTenantId);
        return Expression.Lambda(equality, parameter);
    }
}
