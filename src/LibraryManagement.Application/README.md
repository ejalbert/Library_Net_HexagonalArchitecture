# LibraryManagement.Application

ASP.NET Core host that composes every module (domain, Mongo and Postgres persistence, REST API, OpenAI adapter, and Blazor UI).

## Responsibilities

- Bootstraps the module system via `InitializeApplicationModuleConfiguration()`.
- Registers the Domain, Mongo persistence, Postgres persistence, REST API, OpenAI, and Blazor Web modules.
- Hosts middleware such as HTTPS redirection and delegates endpoint registration to modules (REST, Web).

## Project Layout

```
LibraryManagement.Application/
  Program.cs                     # Module composition root (Add*Module/Use*Module chain)
  appsettings*.json              # Host + module configuration (RestApi, PersistenceMongo, PersistencePostgres, OpenAi)
  Properties/launchSettings.json # Local profiles
```

## Commands

```bash
# Restore dependencies
dotnet restore

# Build the host
dotnet build

# Run the composed API + UI (REST + Blazor + OpenAI integration)
dotnet run
```

## Configuration

- `RestApi:BasePath` – forwarded to the REST delivery module and REST client (defaults to `/api`).
- `ConnectionStrings:mongodb` – preferred MongoDB connection string (fallback: `PersistenceMongo:ConnectionString`).
- `ConnectionStrings:postgres` – preferred Postgres connection string (fallback: `PersistencePostgres:ConnectionString`).
- `PersistenceMongo:DatabaseName` / `PersistencePostgres:DatabaseName` – database names for Mongo and Postgres modules.
- `OpenAi:ApiKey` / `OpenAi:Model` – used by the OpenAI module for book suggestions and AI consumption tracking.
- `Domain:Test` – sample option showing how the Domain module binds configuration (extend/replace once meaningful
  settings exist).

Use `dotnet user-secrets` or environment variables for local overrides when testing against remote infrastructure.

## Tests

`LibraryManagement.Application.Tests` boots the real host with in-memory persistence and covers:

- `/api/v1/books` and `/api/v1/authors` flows end-to-end through REST + domain.
- Blazor shell reachability (`/` + static assets).
- `PersistenceMongoModuleOptions` binding via configuration overrides.

Add coverage for AI suggestion endpoints and tenant-claim wiring as those scenarios evolve.

## Related Modules

- `LibraryManagement.Api.Rest`: maps `/api/v1/books`, `/api/v1/authors`, `/api/v1/ai/book-suggestions`.
- `LibraryManagement.Persistence.Mongo`: implements book/author persistence ports.
- `LibraryManagement.Persistence.Postgres`: EF Core adapter + migrations assembly for book/author persistence and AI consumption logging.
- `LibraryManagement.AI.OpenAi`: OpenAI-backed book suggestion adapter + chat tooling + consumption tracking.
- `LibraryManagement.Web`: serves the Blazor UI that calls the REST API via the shared REST client.

Keep this README updated whenever additional modules are added to the host.
