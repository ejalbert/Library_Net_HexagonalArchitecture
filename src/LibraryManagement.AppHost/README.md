# LibraryManagement.AppHost

Aspire app host that orchestrates the distributed development topology for Library Management. It provisions local
infrastructure containers, wires the application project, and exposes quick-open commands for API documentation UIs.

## Responsibilities

- Defines the Aspire distributed application for local development.
- Provisions MongoDB, PostgreSQL, Redis, RabbitMQ, and PgAdmin resources with persistent volumes.
- Pins container image versions to the latest known stable releases.
- Exposes management UIs for PgAdmin and RabbitMQ through external HTTP endpoints (RabbitMQ uses port 15672).
- Runs Postgres migration and seeding console apps against the Aspire-managed database.
- Registers the application project and attaches OpenAPI UI commands (Swagger, ReDoc, Scalar).

## Dependencies

- `LibraryManagement.Application` for the ASP.NET Core host that serves the APIs.
- Aspire hosting packages for MongoDB, PostgreSQL, RabbitMQ, and Redis.
- Shared architecture guidance: `../../docs/architecture.md`.

## Directory Layout

```
LibraryManagement.AppHost/
  AppHost.cs
  OpenApiUiExstensions.cs
  appsettings.json
  appsettings.Development.json
  Properties/
```

## Commands

```bash
# Build the app host
dotnet build

# Run the distributed app host
dotnet run
```

## Tests

- No dedicated tests yet. Add a matching `LibraryManagement.AppHost.Tests` project if host configuration logic grows.

## Integration Points

- Inbound: none (orchestration only).
- Outbound: containers for MongoDB, PostgreSQL, Redis, RabbitMQ, and PgAdmin.
- Project reference: `LibraryManagement.Application` is the hosted service.
- Project reference: `LibraryManagement.Persistence.Postgres.Migrations` applies EF Core migrations.
- Project reference: `LibraryManagement.Persistence.Postgres.Seeders` seeds initial Postgres data.

## Environment & Configuration

- Uses `appsettings.json` and `appsettings.Development.json` for local configuration.
- Secrets for container credentials are defined as Aspire parameters in `AppHost.cs`.
- Management UIs are reachable via the Aspire dashboard endpoints for PgAdmin and RabbitMQ.

## Related Documentation

- `../../README.md`
- `../../docs/architecture.md`

## Maintenance Notes

- TODO(agent): Add host-level tests if additional orchestration logic is introduced.
