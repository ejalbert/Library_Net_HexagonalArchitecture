# LibraryManagement.Application

ASP.NET Core host that composes every module (domain, Mongo and Postgres persistence, REST API, and Blazor UI).

## Responsibilities

- Bootstraps the module system via `InitializeApplicationModuleConfiguration()`.
- Registers the Domain, Mongo persistence, Postgres persistence, REST API, and Blazor Web modules.
- Hosts middleware such as HTTPS redirection and delegates endpoint registration to modules.

## Project Layout

```
LibraryManagement.Application/
  Program.cs                     # Module composition root
  appsettings*.json              # Host + module configuration (RestApi, PersistenceMongo, PersistencePostgres)
  Properties/launchSettings.json # Local profiles
```

## Commands

```bash
# Restore dependencies
dotnet restore

# Build the host
dotnet build

# Run the composed API + UI
dotnet run
```

## Configuration

- `RestApi:BasePath` – forwarded to the REST delivery module and REST client.
- `PersistenceMongo:ConnectionString` / `DatabaseName` – consumed by the Mongo module when constructing `MongoClient`
  and `IMongoDatabase`.
- `PersistencePostgres:ConnectionString` / `DatabaseName` – consumed by the Postgres module for EF Core configuration
  (defaults align with the `postgres` service in `compose-dev.yaml`).
- `Domain:Test` – sample option showing how the Domain module binds configuration (extend/rename once meaningful
  settings exist).

Use `dotnet user-secrets` or environment variables for local overrides when testing against remote infrastructure.

## Tests

`LibraryManagement.Application.Tests` exists but does not contain scenarios yet. Populate it with ASP.NET Core host
tests (e.g., health checks, module wiring) before shipping functionality.

## Related Modules

- `LibraryManagement.Api.Rest`: maps `/api/v1/books` endpoints.
- `LibraryManagement.Persistence.Mongo`: implements book persistence ports.
- `LibraryManagement.Persistence.Postgres`: EF Core adapter + migrations assembly for book persistence.
- `LibraryManagement.Web`: serves the Blazor UI that calls the REST API via the shared REST client.

Keep this README updated whenever additional modules are added to the host.
