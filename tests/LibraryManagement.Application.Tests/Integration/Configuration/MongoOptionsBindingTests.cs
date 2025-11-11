using LibraryManagement.Application.Tests.Infrastructure;
using LibraryManagement.Persistence.Mongo.ModuleConfigurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LibraryManagement.Application.Tests.Integration.Configuration;

[Collection(ApplicationTestCollection.Name)]
public class MongoOptionsBindingTests
{
    [Fact]
    public void Options_bind_from_environment_variables()
    {
        const string connectionString = "mongodb://integration-tests:27017";
        const string databaseName = "library_integration";

        string? originalConnection = Environment.GetEnvironmentVariable("PersistenceMongo__ConnectionString");
        string? originalDatabase = Environment.GetEnvironmentVariable("PersistenceMongo__DatabaseName");

        try
        {
            Environment.SetEnvironmentVariable("PersistenceMongo__ConnectionString", connectionString);
            Environment.SetEnvironmentVariable("PersistenceMongo__DatabaseName", databaseName);

            using ApplicationWebApplicationFactory factory = new();
            using IServiceScope scope = factory.Services.CreateScope();
            var options = scope.ServiceProvider
                .GetRequiredService<IOptions<PersistenceMongoModuleOptions>>()
                .Value;

            Assert.Equal(connectionString, options.ConnectionString);
            Assert.Equal(databaseName, options.DatabaseName);
        }
        finally
        {
            Environment.SetEnvironmentVariable("PersistenceMongo__ConnectionString", originalConnection);
            Environment.SetEnvironmentVariable("PersistenceMongo__DatabaseName", originalDatabase);
        }
    }
}
