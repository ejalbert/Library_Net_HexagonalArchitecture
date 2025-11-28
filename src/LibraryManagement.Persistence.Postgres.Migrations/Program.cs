// See https://aka.ms/new-console-template for more information

using LibraryManagement.Persistence.Postgres;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
                       ?? builder.Configuration["ConnectionStrings:Default"]
                       ?? builder.Configuration["PersistencePostgres:ConnectionString"]
                       ?? "Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres";

builder.Services.AddDbContext<LibraryManagementDbContext>(options =>
    options.UseNpgsql(connectionString,
        sql => sql.MigrationsAssembly("LibraryManagement.Persistence.Postgres.Migrations")));

IHost app = builder.Build();

await app.StartAsync();
