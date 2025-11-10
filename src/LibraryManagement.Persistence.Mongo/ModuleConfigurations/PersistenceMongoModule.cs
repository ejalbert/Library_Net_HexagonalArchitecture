using LibraryManagement.Domain.Domains.Books.CreateNewBook;
using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;
using LibraryManagement.Persistence.Mongo.Domains.Books;
using LibraryManagement.Persistence.Mongo.Domains.Books.Adapters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.ModuleConfigurations;

public static class PersistenceMongoModule
{
    
    public  static IModuleRegistrator<TApplicationBuilder> AddPersistenceMongoModule<TApplicationBuilder>(this IModuleRegistrator<TApplicationBuilder> moduleRegistrator, Action<PersistenceMongoModuleOptions>? configureOptions = null) where TApplicationBuilder : IHostApplicationBuilder
    {
        PersistenceMongoModuleEnvConfiguration optionsFromEnv = new();
        moduleRegistrator.ConfigurationManager.GetSection("PersistenceMongo").Bind(optionsFromEnv);

        moduleRegistrator.Services.AddOptions<PersistenceMongoModuleOptions>().Configure(options =>
        {
            options.ConnectionString = optionsFromEnv.ConnectionString ?? "mongodb://localhost:20027";
            options.DatabaseName = optionsFromEnv.DatabaseName ?? "library_management";
        });
        
        if(configureOptions != null)
        {
            moduleRegistrator.Services.Configure(configureOptions);
        }
        

        moduleRegistrator.Services.AddSingleton<MongoClient>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<PersistenceMongoModuleOptions>>().Value;
            return new MongoClient(options.ConnectionString);
        }).AddScoped<IMongoDatabase>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<PersistenceMongoModuleOptions>>().Value;
            var mongoClient = serviceProvider.GetRequiredService<MongoClient>();
            
            return mongoClient.GetDatabase(options.DatabaseName);
        });


        moduleRegistrator.Services.AddBookServices();
        
        return moduleRegistrator;
    }
}
