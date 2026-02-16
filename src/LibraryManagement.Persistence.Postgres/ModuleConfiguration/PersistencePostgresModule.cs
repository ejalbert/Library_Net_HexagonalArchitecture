using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;
using LibraryManagement.Persistence.Postgres.DbContexts;
using LibraryManagement.Persistence.Postgres.DbContexts.Multitenants;
using LibraryManagement.Persistence.Postgres.Domains.Ai;
using LibraryManagement.Persistence.Postgres.Domains.Authors;
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
            var connectionString = moduleRegistrator.ConfigurationManager["ConnectionStrings:postgres"];
            options.ConnectionString = connectionString ??
                                       optionsFromEnv.ConnectionString ??
                                       "Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres";
            options.DatabaseName = optionsFromEnv.DatabaseName ?? "library_management";
        });

        if (configureOptions != null) moduleRegistrator.Services.Configure(configureOptions);

        moduleRegistrator.Services.AddScoped<IMultitenantSaveChangesInterceptor, MultitenantSaveChangesInterceptor>();

        moduleRegistrator.Services.AddDbContextFactory<LibraryManagementDbContext>((serviceProvider, options) =>
        {
            PersistencePostgresModuleOptions configs =
                serviceProvider.GetRequiredService<IOptions<PersistencePostgresModuleOptions>>().Value;

            options.UseNpgsql(configs.ConnectionString);
            options.AddInterceptors(serviceProvider.GetRequiredService<IMultitenantSaveChangesInterceptor>());
        }, ServiceLifetime.Scoped);

        moduleRegistrator.Services
            .AddBookServices()
            .AddAuthorServices()
            .AddAiServices();


        return moduleRegistrator;
    }
}
