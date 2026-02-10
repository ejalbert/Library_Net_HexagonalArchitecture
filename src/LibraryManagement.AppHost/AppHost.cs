using Aspire.Hosting.ApplicationModel;

using LibraryManagement.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var postgresUser = builder.AddParameter("postgres-user", "postgres", publishValueAsDefault: true);
var postgresPassword = builder.AddParameter("postgres-password", "postgres", publishValueAsDefault: false, secret: true);

var rabbitUser = builder.AddParameter("rabbitmq-user", "guest", publishValueAsDefault: true);
var rabbitPassword = builder.AddParameter("rabbitmq-password", "guest", publishValueAsDefault: false, secret: true);

var mongo = builder.AddMongoDB("mongo")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var mongodb = mongo.AddDatabase("library-management-mongo");

builder.AddRedis("redis")
    .WithDataVolume();

var postgresService = builder
    .AddPostgres("postgresService", postgresUser, postgresPassword, port: 5432)
    .WithDataVolume()
    .WithPgAdmin()
    .WithExternalHttpEndpoints();

var postgresdb = postgresService
    .AddDatabase("postgres", "library_dev");



builder.AddRabbitMQ("rabbitmq", rabbitUser, rabbitPassword)
    .WithDataVolume()
    .WithManagementPlugin()
    .WithExternalHttpEndpoints();

var application = builder.AddProject<Projects.LibraryManagement_Application>("application")
    .WithReference(mongodb)
    .WithReference(postgresdb)
    .WithSwagger(path:"/dev-ui/swagger")
    .WithRedoc(path:"/dev-ui/api-docs")
    .WithScalar(path:"/dev-ui/scalar");

builder.AddProject<Projects.LibraryManagement_Persistence_Postgres_Seeders>("postgres-seeders")
    .WithExplicitStart()
    .WithReference(postgresdb);

var app = builder.Build();

app.Run();
