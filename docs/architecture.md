# Architecture Guide

The Library Management System follows Hexagonal (Ports & Adapters) architecture. All capabilities are expressed through domain use cases and outbound ports; adapters implement those ports and are composed at runtime via the module bootstrapper.

## Layering Overview

| Layer | Projects | Responsibilities |
| --- | --- | --- |
| Domain | `LibraryManagement.Domain` | Aggregates, commands, use cases, and port interfaces. Covers books (create/search/get/update/patch/delete), authors (create/search), AI book suggestions, AI consumption tracking, and tenant identification. |
| Application Host | `LibraryManagement.Application` | ASP.NET Core host that bootstraps every module (domain, Mongo, Postgres, REST, OpenAI, Blazor). |
| Infrastructure (Driven adapters) | `LibraryManagement.Persistence.Mongo`, `LibraryManagement.Persistence.Postgres`, `LibraryManagement.AI.OpenAi` | MongoDB implementation of book/author ports; EF Core Postgres implementation with multi-tenancy + AI consumption logging; OpenAI adapters for book suggestions and chat tool orchestration. |
| Delivery (Driving adapters) | `LibraryManagement.Api.Rest`, `LibraryManagement.Web`/`.Client` | REST minimal APIs (`/api/v1/books`, `/api/v1/authors`, `/api/v1/ai/book-suggestions`) and Blazor UI (book listings, author creation, AI suggestions) that drive use cases via the REST client. |
| Shared Contracts | `LibraryManagement.Api.Rest.Client` | DTOs + typed HTTP clients for books, authors, and AI book suggestions; keeps server/client contracts aligned. |
| Bootstrapper | `LibraryManagement.ModuleBootstrapper*` | Module registration/configuration abstractions shared across hosts. |

## Module Composition

1. Hosts call `builder.InitializeApplicationModuleConfiguration()` to receive an `IModuleRegistrator`.
2. Modules expose `Add*Module()` extensions that register services and bind options (`DomainModule`, `PersistenceMongoModule`, `ApiModule`, `WebModule`, etc.).
3. After building the app, hosts call `app.UseApplicationModules()` to obtain an `IModuleConfigurator` and chain `Use*Module()` extensions. Delivery modules register endpoints/middleware at this stage.

This pattern keeps service registration and endpoint configuration consistent whether the host is ASP.NET Core, a worker, or another runtime.

## Current Modules

- **Domain** – Books and authors aggregates, AI book suggestions, AI consumption tracking, tenant services, and shared search abstractions. Outbound ports feed persistence and AI adapters.
- **Mongo Persistence** – Registers `MongoClient`, `IMongoDatabase`, author/book collections, Mapperly mappers, and adapters for create/search/get/update/patch/delete. Defaults to `mongodb://localhost:20027` / `library_management`. Tested via Testcontainers.
- **Postgres Persistence** – Registers `LibraryManagementDbContext` with multi-tenant save interceptor, adapters for books/authors, and AI consumption logging. Ships a paired migrations assembly.
- **OpenAI Module** – Configures `ChatClient`, chat tools for searching books/authors, AI book suggestion agent, and consumption tracking.
- **REST Delivery** – Adds OpenAPI and maps `/api/v1/books`, `/api/v1/authors`, and `/api/v1/ai/book-suggestions`. Bridges tenant IDs from the `tenant_id` claim to the domain port.
- **REST Client** – Provides typed clients for books, authors, and AI book suggestions plus DI helpers so other modules (e.g., Blazor) can call the REST API without duplicating contracts.
- **Web Module** – Configures Razor/Blazor services, registers the REST client, and maps UI routes. The Home page fetches books and AI suggestions; the Authors page handles author creation.

## Configuration Map

| Section | Consumer | Default |
| --- | --- | --- |
| `RestApi:BasePath` | REST delivery module & REST client | `/api` (delivery), `http://localhost:5007/api` (client) |
| `PersistenceMongo:ConnectionString` | Mongo module | `mongodb://localhost:20027` |
| `PersistenceMongo:DatabaseName` | Mongo module | `library_management` |
| `PersistencePostgres:ConnectionString` | Postgres module | `Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres` |
| `PersistencePostgres:DatabaseName` | Postgres module | `library_management` |
| `OpenAi:ApiKey` / `OpenAi:Model` | OpenAI module | `""` (key) / `gpt-4.1-nano` (model) |
| `Domain:*` | Domain module | Sample `Test` option (extend/replace as needed) |

Always document new configuration keys in the relevant README + this table.

## Testing Strategy

- **Domain** – Unit tests cover book CRUD/patch flows and authors; add AI and tenant coverage next.
- **Mongo Persistence** – xUnit + Testcontainers integration tests assert author/book adapters persist and query correctly.
- **Postgres Persistence** – xUnit + Testcontainers integration tests assert author/book adapters and multi-tenant enforcement; add AI consumption coverage next.
- **REST Delivery** – xUnit tests validate module registration, option binding, DI wiring, and endpoint routing (books/authors/AI) using `WebApplicationBuilder`.
- **REST Client** – xUnit tests intercept HTTP calls via custom handlers for books/authors; add AI client tests next.
- **Blazor UI** – bUnit tests reside in `.razor` files with `.razor.cs` partials (see `BookPageTests` / `AuthorPageTests`) so discovery works.
- **Integrated Host** – Application tests boot the real host with in-memory persistence to exercise REST + Blazor wiring.

When adding new adapters, mirror them with a `{Project}.Tests` project and describe the coverage in its README.

## UI Considerations

- The Blazor Server app (`LibraryManagement.Web`) doubles as a hybrid host for Blazor WebAssembly by using `.AddInteractiveServerComponents()` and `.AddInteractiveWebAssemblyComponents()`.
- The Web client registers `AddWebClientModule()` to ensure the REST client base address matches the API.
- Components currently include a Home page with AI book suggestions and author listing/navigation plus an Author creation flow.
- Component tests should emulate the pattern used by `BookPageTests`: `@inherits TestContext`, register dependencies (e.g., stubbed `IRestAPiClient`) inside the test body, and keep logic inside `.razor` while the partial `.razor.cs` remains empty.

## Multitenancy Enforcement

The Postgres persistence layer enforces tenant isolation using a SaveChanges interceptor (`MultitenantSaveChangesInterceptor`). This interceptor sets the `TenantId` property on all entities at save time, based on the current user's tenant ID (from `IGetCurrentUserTenantIdUseCase`).

**Testing Pattern:**
- Use separate `DbContext` instances with different mocks of `IGetCurrentUserTenantIdUseCase` to insert/query entities for different tenants.
- The interceptor always overrides `TenantId` on save, so tests must use context-specific mocks to simulate multiple tenants.
- See ADR `0001-multitenancy-enforcement-and-testing.md` for rationale and details.

This ensures strict tenant isolation and robust test coverage for multitenancy boundaries.

## ADRs & Documentation

Record significant decisions inside `docs/adr/`. Keep the README of every project in sync with the code they describe, and update `docs/project-roadmap.md` whenever milestones move forward.
