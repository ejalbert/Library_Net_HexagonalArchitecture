// See https://aka.ms/new-console-template for more information

using LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;
using LibraryManagement.Persistence.Postgres.DbContexts;
using LibraryManagement.Persistence.Postgres.Migrations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

ContextFactory.ConnectionString = builder.Configuration["ConnectionStrings:postgres"]
                                  ?? builder.Configuration.GetConnectionString("Default")
                                  ?? builder.Configuration["ConnectionStrings:Default"]
                                  ?? builder.Configuration["PersistencePostgres:ConnectionString"]
                                  ?? ContextFactory.ConnectionString;


builder.Services.AddDbContext<LibraryManagementDbContext>(options =>
    options.UseNpgsql(ContextFactory.ConnectionString,
        sql => sql.MigrationsAssembly("LibraryManagement.Persistence.Postgres.Migrations")));

IHost app = builder.Build();

await app.StartAsync();

namespace LibraryManagement.Persistence.Postgres.Migrations
{
    public class ContextFactory : IDesignTimeDbContextFactory<LibraryManagementDbContext>
    {
        internal static string ConnectionString =
            "Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres";

        public LibraryManagementDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryManagementDbContext>();

            optionsBuilder.UseNpgsql(ConnectionString,
                sql => sql.MigrationsAssembly("LibraryManagement.Persistence.Postgres.Migrations"));

            return new LibraryManagementDbContext(optionsBuilder.Options, new TenantProvider());
        }
    }

    internal class TenantProvider : IGetCurrentUserTenantIdUseCase
    {
        public string GetTenantId(GetCurrentUserTenantIdCommand command)
        {
            return "00000000-0000-0000-0000-000000000000";
        }
    }
}
