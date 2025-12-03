// See https://aka.ms/new-console-template for more information

using LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;
using LibraryManagement.Persistence.Postgres.DbContexts;
using LibraryManagement.Persistence.Postgres.DbContexts.Multitenants;
using LibraryManagement.Persistence.Postgres.Seeders;
using LibraryManagement.Persistence.Postgres.Seeders.Domain.Authors;
using LibraryManagement.Persistence.Postgres.Seeders.Domain.Books;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
                       ?? builder.Configuration["ConnectionStrings:Default"]
                       ?? builder.Configuration["PersistencePostgres:ConnectionString"]
                       ?? "Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres";



Console.WriteLine("Starting Postgres seeder...");
Console.WriteLine($"Using connection string: {connectionString}");

builder.Services.AddScoped<IGetCurrentUserTenantIdUseCase, TenantProvider>();
builder.Services.AddScoped<IMultitenantSaveChangesInterceptor, MultitenantSaveChangesInterceptor>();

builder.Services.AddDbContext<LibraryManagementDbContext>((sp, options) =>
{
    options
        .UseNpgsql(connectionString)
        .AddInterceptors(sp.GetRequiredService<IMultitenantSaveChangesInterceptor>())
        .UseSeeding((context, _) =>
        {
            Console.WriteLine("Seeding authors...");
            context.SeedAuthors();
            Console.WriteLine("Authors seeded.");

            Console.WriteLine("Seeding books...");
            context.SeedBooks();
            Console.WriteLine("Books seeded.");
        });
});


IHost app = builder.Build();


var dbContext = app.Services.GetRequiredService<LibraryManagementDbContext>();
dbContext.Database.EnsureCreated();

Console.WriteLine("Applying Seedings...");

await app.StartAsync();
Console.WriteLine("Seeding complete. Application exiting.");

// public class ContextFactory : IDesignTimeDbContextFactory<LibraryManagementDbContext>
// {
//     public LibraryManagementDbContext CreateDbContext(string[] args)
//     {
//         var optionsBuilder = new DbContextOptionsBuilder<LibraryManagementDbContext>();
//
//         var connectionString = "Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres";
//
//         optionsBuilder.UseNpgsql(connectionString);
//         optionsBuilder.AddInterceptors()
//
//         return new LibraryManagementDbContext(optionsBuilder.Options, new TenantProvider());
//     }
// }

namespace LibraryManagement.Persistence.Postgres.Seeders
{
    internal class TenantProvider : IGetCurrentUserTenantIdUseCase
    {
        public string GetTenantId(GetCurrentUserTenantIdCommand command)
        {
            return "00000000-0000-0000-0000-000000000001";
        }
    }
}
