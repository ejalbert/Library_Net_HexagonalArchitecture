using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;
using LibraryManagement.Persistence.Postgres.Domains.Books;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace LibraryManagement.Persistence.Postgres.ModuleConfiguration;

public static class PersistencePostgresModule
{
    public static IModuleRegistrator<TApplicationBuilder> AddPersistencePostgresModule<TApplicationBuilder>(
        this IModuleRegistrator<TApplicationBuilder> moduleRegistrator,
        Action<PersistencePostgresModuleOptions>? configureOptions = null)
        where TApplicationBuilder : IHostApplicationBuilder
    {
        PersistencePostgresModuleEnvConfiguration optionsFromEnv = new();
        moduleRegistrator.ConfigurationManager.GetSection("PersistencePostgres").Bind(optionsFromEnv);

        moduleRegistrator.Services.AddOptions<PersistencePostgresModuleOptions>().Configure(options =>
        {
            options.ConnectionString = optionsFromEnv.ConnectionString ??
                                       "Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres";
            options.DatabaseName = optionsFromEnv.DatabaseName ?? "library_management";
        });

        if (configureOptions != null) moduleRegistrator.Services.Configure(configureOptions);

        moduleRegistrator.Services.AddDbContextFactory<LibraryManagementDbContext>((serviceProvider, options) =>
        {
            PersistencePostgresModuleOptions configs =
                serviceProvider.GetRequiredService<IOptions<PersistencePostgresModuleOptions>>().Value;

            options.UseNpgsql(configs.ConnectionString,
                o => { o.MigrationsAssembly("LibraryManagement.Persistence.Postgres.Migrations"); });
        }, ServiceLifetime.Scoped);

        moduleRegistrator.Services
            .AddBookServices();
        //     .AddAuthorServices()
        //     .AddBookServices();

        return moduleRegistrator;
    }
}
