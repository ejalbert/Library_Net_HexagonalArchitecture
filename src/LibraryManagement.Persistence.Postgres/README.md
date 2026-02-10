# LibraryManagement.Persistence.Postgres

Entity Framework Core Postgres persistence module for the Library Management services. The current surface covers the
book create/get/update/patch/delete ports; extend it alongside new domain use cases.

## Prerequisites

- Docker with `docker compose`
- Access to the dev Postgres service defined in `compose-dev.yaml`

## Default connection

- Host: localhost
- Port: 5432
- Database: library_dev
- Username: postgres
- Password: postgres
- Connection string: `Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres`

When running inside the compose network, use host `postgres` instead of `localhost`.

## Running Postgres locally

1. `docker compose -f compose-dev.yaml up -d postgres`
2. Optional: start pgAdmin too with `docker compose -f compose-dev.yaml up -d pgadmin` and connect using host
   `postgres`, port `5432`, user `postgres`, password `postgres`.

## Configuration

The module prefers `ConnectionStrings:postgres` for the connection string, falling back to `PersistencePostgres`.

Example configuration:

```json
{
  "ConnectionStrings": {
    "postgres": "Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres"
  },
  "PersistencePostgres": {
    "DatabaseName": "library_management"
  }
}
```

Values can be supplied via appsettings or environment variables (e.g., `ConnectionStrings__postgres` or
`PersistencePostgres__ConnectionString`).

## Usage

Register the module in your host builder:

```csharp
builder.AddPersistencePostgresModule();
```

`LibraryManagementDbContext` exposes the EF Core sets (e.g., `Books`) and uses `UseNpgsql` with the configured
connection string and migrations assembly.

## Database migrations

Migrations live in `src/LibraryManagement.Persistence.Postgres.Migrations`.

```bash
# Add a migration targeting the migrations assembly
dotnet ef migrations add <Name> \
  --project src/LibraryManagement.Persistence.Postgres.Migrations/LibraryManagement.Persistence.Postgres.Migrations.csproj \
  --startup-project src/LibraryManagement.Persistence.Postgres.Migrations/LibraryManagement.Persistence.Postgres.Migrations.csproj \
  --output-dir Migrations

# Apply migrations to the configured database
dotnet ef database update \
  --project src/LibraryManagement.Persistence.Postgres.Migrations/LibraryManagement.Persistence.Postgres.Migrations.csproj \
  --startup-project src/LibraryManagement.Persistence.Postgres.Migrations/LibraryManagement.Persistence.Postgres.Migrations.csproj
```

Ensure the connection string (appsettings or `PersistencePostgres__ConnectionString`) points to a running Postgres
instance before running migrations.

## Multitenancy Enforcement

This module enforces tenant isolation using an EF Core SaveChanges interceptor (`MultitenantSaveChangesInterceptor`). All entities' `TenantId` properties are set at save time based on the current user's tenant ID (from `IGetCurrentUserTenantIdUseCase`).

**Testing Guidance:**
- Use separate `DbContext` instances with different mocks of `IGetCurrentUserTenantIdUseCase` to insert/query entities for different tenants.
- The interceptor always overrides `TenantId` on save, so tests must use context-specific mocks to simulate multiple tenants.
- See `docs/adr/0001-multitenancy-enforcement-and-testing.md` for details.
