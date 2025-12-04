using LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;
using LibraryManagement.Persistence.Postgres.DbContexts.Multitenants;
using LibraryManagement.Persistence.Postgres.Domains.Ai.AiConsumption;
using LibraryManagement.Persistence.Postgres.Domains.Authors;
using LibraryManagement.Persistence.Postgres.Domains.Books;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.DbContexts;

public class LibraryManagementDbContext(DbContextOptions<LibraryManagementDbContext> options, IGetCurrentUserTenantIdUseCase getCurrentUserTenantIdUseCase) : MultitenantDbContext(options, getCurrentUserTenantIdUseCase)
{
    public DbSet<BookEntity> Books { get; set; } = null!;
    public DbSet<AuthorEntity> Authors { get; set; } = null!;

    public DbSet<AiConsumptionEntity> TenantAiConsumptions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureBooks().ConfigureAuthors().ConfigureAiConsumptions();
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres");
    // }
}
