## LibraryManagement.Persistence.Postgres

Entity Framework Core Postgres persistence module for the Library Management services.

### Prerequisites

- Docker with `docker compose`
- Access to the dev Postgres service defined in `compose-dev.yaml`

### Default connection

- Host: localhost
- Port: 5432
- Database: library_dev
- Username: postgres
- Password: postgres
- Connection string: `Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres`

When running inside the compose network, use host `postgres` instead of `localhost`.

### Running Postgres locally

1. `docker compose -f compose-dev.yaml up -d postgres`
2. Optional: start pgAdmin too with `docker compose -f compose-dev.yaml up -d pgadmin` and connect using host
   `postgres`, port `5432`, user `postgres`, password `postgres`.

### Configuration

The module binds to `PersistencePostgres` configuration:

```json
{
  "PersistencePostgres": {
    "ConnectionString": "Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres",
    "DatabaseName": "library_management"
  }
}
```

Values can be supplied via appsettings or environment variables (e.g., `PersistencePostgres__ConnectionString`).

### Usage

Register the module in your host builder:

```csharp
builder.AddPersistencePostgresModule();
```

`LibraryManagementDbContext` exposes the EF Core sets (e.g., `Books`) and uses `UseNpgsql` with the configured
connection string.

### Database migrations

- Install EF tools if needed: `dotnet tool install --global dotnet-ef`
- From this project directory (`src/LibraryManagement.Persistence.Postgres`), add a migration targeting the bootstrapper
  startup:

```bash
dotnet ef migrations add InitialCreate
```

- Apply migrations to the dev database:

```bash
dotnet ef database update --startup-project ../LibraryManagement.ModuleBootstrapper
```

Ensure the connection string (appsettings or `PersistencePostgres__ConnectionString`) points to a running Postgres
instance before running migrations.
