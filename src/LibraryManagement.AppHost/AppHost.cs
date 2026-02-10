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

var postgres = builder.AddPostgres("postgres", postgresUser, postgresPassword, port: 5432)
    .WithDataVolume();

var postgresdb = postgres.AddDatabase("library-dev", "library_dev");

builder.AddRabbitMQ("rabbitmq", rabbitUser, rabbitPassword)
    .WithDataVolume()
    .WithManagementPlugin();

builder.AddContainer("pgadmin", "dpage/pgadmin4:9.10.0")
    .WithEnvironment("PGADMIN_DEFAULT_EMAIL", "admin@local.com")
    .WithEnvironment("PGADMIN_DEFAULT_PASSWORD", "admin")
    .WithEnvironment("PGADMIN_CONFIG_SERVER_MODE", "False")
    .WithHttpEndpoint(targetPort: 80, port: 5050, name: "pgadmin")
    .WithExternalHttpEndpoints()
    .WithVolume("pgadmin-data", "/var/lib/pgadmin");

var application = builder.AddProject<Projects.LibraryManagement_Application>("application")
    .WithReference(mongodb)
    .WithReference(postgresdb)
    .WithSwagger()
    .WithRedoc()
    .WithScalar();

builder.Build().Run();


