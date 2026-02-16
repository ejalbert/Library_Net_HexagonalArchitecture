using Aspire.Hosting.JavaScript;

using LibraryManagement.AppHost.Extensions;

using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ParameterResource> postgresUser = builder.AddParameter("postgres-user", "postgres", true);
IResourceBuilder<ParameterResource> postgresPassword =
    builder.AddParameter("postgres-password", "postgres", false, true);

IResourceBuilder<ParameterResource> rabbitUser = builder.AddParameter("rabbitmq-user", "guest", true);
IResourceBuilder<ParameterResource> rabbitPassword = builder.AddParameter("rabbitmq-password", "guest", false, true);

IResourceBuilder<MongoDBServerResource> mongo = builder.AddMongoDB("mongo")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<MongoDBDatabaseResource> mongodb = mongo.AddDatabase("library-management-mongo");

builder.AddRedis("redis")
    .WithDataVolume();

IResourceBuilder<PostgresServerResource> postgresService = builder
    .AddPostgres("postgresService", postgresUser, postgresPassword, 5432)
    .WithDataVolume()
    .WithPgAdmin()
    .WithExternalHttpEndpoints();

IResourceBuilder<PostgresDatabaseResource> postgresdb = postgresService
    .AddDatabase("postgres", "library_dev")
    .WithCreateNewMigrationCommand()
    .WithRemoveMigrationCommand()
    .WithMigrateDatabaseCommand()
    .WithRevertAllMigrationsCommand();


builder.AddRabbitMQ("rabbitmq", rabbitUser, rabbitPassword)
    .WithDataVolume()
    .WithManagementPlugin()
    .WithExternalHttpEndpoints();

IResourceBuilder<ProjectResource> application = builder.AddProject<LibraryManagement_Application>("application")
    .WithReference(mongodb)
    .WithReference(postgresdb)
    .WaitFor(postgresdb)
    .WaitFor(mongodb)
    .WithSwagger(path: "/dev-ui/swagger")
    .WithRedoc(path: "/dev-ui/api-docs")
    .WithScalar(path: "/dev-ui/scalar");

builder.AddProject<LibraryManagement_Persistence_Postgres_Seeders>("postgres-seeders")
    .WithExplicitStart()
    .WithReference(postgresdb)
    .WaitFor(postgresService);

IResourceBuilder<ViteAppResource> reactClient = builder.AddViteApp("react-client", "../LibraryManagement.Web.React")
    .WithReference(application)
    .WithGenerateApiClientCommand(application);


DistributedApplication app = builder.Build();

app.Run();
