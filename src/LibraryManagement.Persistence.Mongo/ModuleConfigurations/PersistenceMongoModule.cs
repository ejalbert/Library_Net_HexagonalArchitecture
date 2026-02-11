using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;
using LibraryManagement.Persistence.Mongo.Domains.Authors;
using LibraryManagement.Persistence.Mongo.Domains.Books;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.ModuleConfigurations;

public static class PersistenceMongoModule
{
    public static IModuleRegistrator<TApplicationBuilder> AddPersistenceMongoModule<TApplicationBuilder>(
        this IModuleRegistrator<TApplicationBuilder> moduleRegistrator,
        Action<PersistenceMongoModuleOptions>? configureOptions = null)
        where TApplicationBuilder : IHostApplicationBuilder
    {
        PersistenceMongoModuleEnvConfiguration optionsFromEnv = new();
        moduleRegistrator.ConfigurationManager.GetSection("PersistenceMongo").Bind(optionsFromEnv);

        moduleRegistrator.Services.AddOptions<PersistenceMongoModuleOptions>().Configure(options =>
        {
            string? connectionString = moduleRegistrator.ConfigurationManager["ConnectionStrings:mongodb"];
            options.ConnectionString = connectionString ?? optionsFromEnv.ConnectionString ?? "mongodb://localhost:20027";
            options.DatabaseName = optionsFromEnv.DatabaseName ?? "library_management";
        });

        if (configureOptions != null) moduleRegistrator.Services.Configure(configureOptions);


        moduleRegistrator.Services.AddSingleton<MongoClient>(serviceProvider =>
        {
            PersistenceMongoModuleOptions options =
                serviceProvider.GetRequiredService<IOptions<PersistenceMongoModuleOptions>>().Value;
            return new MongoClient(options.ConnectionString);
        }).AddScoped<IMongoDatabase>(serviceProvider =>
        {
            PersistenceMongoModuleOptions options =
                serviceProvider.GetRequiredService<IOptions<PersistenceMongoModuleOptions>>().Value;
            MongoClient mongoClient = serviceProvider.GetRequiredService<MongoClient>();

            return mongoClient.GetDatabase(options.DatabaseName);
        });


        moduleRegistrator.Services
            .AddAuthorServices()
            .AddBookServices();

        return moduleRegistrator;
    }
}
