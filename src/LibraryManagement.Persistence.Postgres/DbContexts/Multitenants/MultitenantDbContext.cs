using System.Linq.Expressions;

using LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;
using LibraryManagement.Persistence.Postgres.DbContexts.Multitenants.Domains.Tenants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LibraryManagement.Persistence.Postgres.DbContexts.Multitenants;

public abstract class MultitenantDbContext : DbContext
{
    public DbSet<TenantEntity> Tenants { get; set; } = null!;

    protected MultitenantDbContext(DbContextOptions options, IGetCurrentUserTenantIdUseCase getCurrentUserTenantIdUseCase)
        : base(options)
    {
        CurrentTenantId = Guid.Parse(getCurrentUserTenantIdUseCase.GetTenantId(new()));
    }

    internal Guid CurrentTenantId { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ReplaceService<IModelCacheKeyFactory, MultitenantModelCacheKeyFactory>();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureTenants();

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IMultitenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                var typeBuilder = modelBuilder.Entity(entityType.ClrType);


                typeBuilder.HasOne(nameof(IMultitenantEntity.Tenant))
                    .WithMany()
                    .HasForeignKey(nameof(IMultitenantEntity.TenantId))
                    .IsRequired();

                typeBuilder.HasQueryFilter("Filter by tenant", CreateTenantFilterExpression(entityType.ClrType, CurrentTenantId));
            }
        }
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
