# Hexagonal Library Management System

A .NET 10 reference implementation of a modular Library Management System centred on Hexagonal (Ports & Adapters) architecture. The solution now delivers a book + author slice end-to-end with OpenAI-powered suggestions: domain use cases, MongoDB and Postgres persistence adapters, a REST delivery module, a typed REST client, and a Blazor UI that consumes the API.

## Current Status

- ✅ Module bootstrapper libraries (`LibraryManagement.ModuleBootstrapper*`) let any host compose modules in a consistent way.
- ✅ Domain layer covers books (create/search/get/update/patch/delete), authors (create/search), AI book suggestions, AI consumption tracking, and tenant identification.
- ✅ MongoDB adapter persists books and authors with Mapperly mappings plus Testcontainers coverage.
- ✅ Postgres adapter (EF Core) persists books, authors, and AI consumption entries with multi-tenant enforcement and a dedicated migrations assembly.
- ✅ REST API module maps `/api/v1/books`, `/api/v1/authors`, and `/api/v1/ai/book-suggestions` endpoints, publishes OpenAPI metadata, and bridges tenant IDs from HTTP claims.
- ✅ REST client package ships typed clients for books, authors, and AI book suggestions plus DI-friendly configuration helpers.
- ✅ Blazor host (`LibraryManagement.Web` + `.Client`) renders book listings, author creation, and an AI suggestion panel backed by the REST client.
- 🚧 Additional domains (patrons, circulation, policies) still need to be modelled.

See `docs/project-roadmap.md` for upcoming milestones.

## Solution Layout

```
src/
  LibraryManagement.Application           # ASP.NET Core host that composes every module
  LibraryManagement.Domain                # Domain model + use cases (books, authors, AI, tenants)
  LibraryManagement.AI.OpenAi             # OpenAI-driven outbound adapters (book suggestions, chat tools, consumption logging)
  LibraryManagement.Persistence.Mongo     # MongoDB outbound adapters
  LibraryManagement.Persistence.Postgres  # EF Core Postgres outbound adapters (books, authors, AI consumption)
  LibraryManagement.Persistence.Postgres.Migrations # EF Core migrations assembly for Postgres
  LibraryManagement.Api.Rest              # REST delivery module (minimal APIs)
  LibraryManagement.Api.Rest.Client       # Shared DTOs + typed HttpClient wrappers
  LibraryManagement.Web                   # Blazor Server entry point
  LibraryManagement.Web/LibraryManagement.Web.Client  # Blazor WebAssembly client bundle
  LibraryManagement.ModuleBootstrapper*   # Module registration/configuration helpers

tests/
  LibraryManagement.Persistence.Mongo.Tests        # Integration + mapping tests
  LibraryManagement.Persistence.Postgres.Tests     # Integration harness for EF Core adapters
  LibraryManagement.Api.Rest.Tests                 # Module registration and routing tests
  LibraryManagement.Api.Rest.Client.Tests          # Typed client contract tests
  LibraryManagement.Web.Tests                      # bUnit component tests (Book/Author pages)
  LibraryManagement.ModuleBootstrapper*.Tests      # Bootstrapper regression tests
  LibraryManagement.Application.Tests              # Placeholder for future host tests
  LibraryManagement.Tests.Abstractions             # Shared fixtures/helpers for test projects
```

## Running the Application

```bash
# Restore every project
dotnet restore

# Run the integrated host (REST API + Mongo/Postgres + Blazor + OpenAI)
dotnet run --project src/LibraryManagement.Application/LibraryManagement.Application.csproj
```

The host wires modules in `Program.cs` via `InitializeApplicationModuleConfiguration()` and the fluent `Add*Module()` extensions. Make sure backing dependencies are running (e.g., `docker compose -f compose-dev.yaml up -d mongo postgres`). Configuration is read from the usual ASP.NET Core providers:

- `RestApi` section -> REST base path (defaults to `/api`); also used by the REST client to set the typed `HttpClient` base address.
- `PersistenceMongo` section -> Mongo connection string + database (defaults to `mongodb://localhost:20027`, `library_management`).
- `PersistencePostgres` section -> Postgres connection string + database (defaults to `Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres`).
- `OpenAi` section -> OpenAI API key + model (defaults to `gpt-4.1-nano`); required for `/api/v1/ai/book-suggestions` and AI consumption logging.

Use `compose.yaml` to build the Blazor server and WebAssembly client containers once you are ready to deploy UI artifacts independently.

## Tests

```bash
# MongoDB adapters (uses Testcontainers -> requires Docker running)
dotnet test tests/LibraryManagement.Persistence.Mongo.Tests/LibraryManagement.Persistence.Mongo.Tests.csproj

# Postgres adapters (uses Testcontainers -> requires Docker running)
dotnet test tests/LibraryManagement.Persistence.Postgres.Tests/LibraryManagement.Persistence.Postgres.Tests.csproj

# Domain unit tests
dotnet test tests/LibraryManagement.Domain.Tests/LibraryManagement.Domain.Tests.csproj

# REST API module behaviour
dotnet test tests/LibraryManagement.Api.Rest.Tests/LibraryManagement.Api.Rest.Tests.csproj

# Typed REST client contracts
dotnet test tests/LibraryManagement.Api.Rest.Client.Tests/LibraryManagement.Api.Rest.Client.Tests.csproj

# Blazor components (bUnit)
dotnet test tests/LibraryManagement.Web.Tests/LibraryManagement.Web.Tests.csproj

# Integrated host (in-memory persistence)
dotnet test tests/LibraryManagement.Application.Tests/LibraryManagement.Application.Tests.csproj
```

> **Blazor testing rule** – Define each component test as a `.razor` file with a matching partial `.razor.cs` code-behind so the bUnit test runner can discover it (see `tests/LibraryManagement.Web.Tests/Components/BookPageTests.razor*`).

## Development Workflow

1. Add or update domain logic first (entities, ports, use cases) inside `LibraryManagement.Domain`.
2. Register outbound adapters (e.g., Mongo) via module extensions so hosts stay ignorant of infrastructure details.
3. Surface capabilities through delivery adapters such as the REST module and Blazor UI.
4. Mirror each project with a `{Project}.Tests` counterpart; integration adapters should exercise real dependencies through Testcontainers when possible.
5. Keep docs (`docs/architecture.md`, `docs/ai-collaboration.md`, ADRs) current whenever behaviour changes.
6. For REST contracts, annotate DTOs and path parameters with `System.ComponentModel.Description` so OpenAPI metadata is consistent.

### Tooling

- .NET SDK 10 preview (align with the `TargetFramework` in each `.csproj`).
- MongoDB 7 (local instance or container) for manual testing; automated tests run against Testcontainers.
- xUnit + FluentAssertions + Moq in most test projects; bUnit for component tests.
- Mapperly generates DTO/entity mappers inside Domain and Persistence projects.
- Local NuGet feed (`NuGet.config`) registers `./local-libraries`, so place private `.nupkg`/`.tgz` artifacts there and run `dotnet nuget list source` if a restore cannot find them.

## Documentation

- **Architecture**: `docs/architecture.md` – layering rules, module catalogue, deployment considerations.
- **Roadmap**: `docs/project-roadmap.md` – milestone tracking with the current state checked off.
- **AI Collaboration**: `docs/ai-collaboration.md` – how humans and agents should work together (includes the Blazor testing rule).
- **ADRs**: `docs/adr/` – record significant decisions.

## Multitenancy Enforcement

The Postgres persistence module enforces strict tenant isolation using an EF Core SaveChanges interceptor. All entities' `TenantId` properties are set at save time based on the current user's tenant ID (via `IGetCurrentUserTenantIdUseCase`, supplied by the REST adapter from the `tenant_id` claim and defaulting to `00000000-0000-0000-0000-000000000001` when absent).

**Testing Guidance:**
- Use separate `DbContext` instances with different mocks of `IGetCurrentUserTenantIdUseCase` to insert/query entities for different tenants.
- The interceptor always overrides `TenantId` on save, so tests must use context-specific mocks to simulate multiple tenants.
- See `docs/adr/0001-multitenancy-enforcement-and-testing.md` for details.

Please keep READMEs and docs synchronized with the code whenever behaviour or dependencies change so future contributors (human or AI) can trust this repository.
