using LibraryManagement.Persistence.Postgres.Domains.Books;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres;

public class LibraryManagementDbContext(DbContextOptions<LibraryManagementDbContext> options) : DbContext(options)
{
    public DbSet<BookEntity> Books { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureBooks();
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres");
    // }
}
