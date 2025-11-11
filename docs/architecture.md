# Architecture Guide

The Library Management System follows Hexagonal (Ports & Adapters) architecture. All capabilities are expressed through domain use cases and outbound ports; adapters implement those ports and are composed at runtime via the module bootstrapper.

## Layering Overview

| Layer | Projects | Responsibilities |
| --- | --- | --- |
| Domain | `LibraryManagement.Domain` | Aggregates, commands, use cases, and port interfaces. Currently focuses on the `Books` aggregate (create/search/get). |
| Application Host | `LibraryManagement.Application` | ASP.NET Core host that bootstraps every module and exposes HTTP/Blazor endpoints. |
| Infrastructure (Driven adapters) | `LibraryManagement.Persistence.Mongo` | MongoDB implementation of the outbound ports. Uses Mapperly to convert between domain models and Mongo entities. |
| Delivery (Driving adapters) | `LibraryManagement.Api.Rest`, `LibraryManagement.Web`/`.Client` | REST minimal APIs and Blazor UI that drive use cases via the REST client. |
| Shared Contracts | `LibraryManagement.Api.Rest.Client` | DTOs + typed HTTP clients that keep server/client contracts aligned. |
| Bootstrapper | `LibraryManagement.ModuleBootstrapper*` | Module registration/configuration abstractions shared across hosts. |

## Module Composition

1. Hosts call `builder.InitializeApplicationModuleConfiguration()` to receive an `IModuleRegistrator`.
2. Modules expose `Add*Module()` extensions that register services and bind options (`DomainModule`, `PersistenceMongoModule`, `ApiModule`, `WebModule`, etc.).
3. After building the app, hosts call `app.UseApplicationModules()` to obtain an `IModuleConfigurator` and chain `Use*Module()` extensions. Delivery modules register endpoints/middleware at this stage.

This pattern keeps service registration and endpoint configuration consistent whether the host is ASP.NET Core, a worker, or another runtime.

## Current Modules

- **Domain** – `Book` entity plus use cases. Relies on outbound ports for persistence.
- **Mongo Persistence** – Registers `MongoClient`, `IMongoDatabase`, `BookCollection`, mapper, and adapters. Defaults to `mongodb://localhost:20027` / `library_management`. Tested via Testcontainers.
- **REST Delivery** – Adds OpenAPI and maps `/api/v1/books` create/get/search endpoints using DTOs from the REST client package.
- **REST Client** – Provides `IBooksClient`, DTOs, and DI helpers so other modules (e.g., Blazor) can call the REST API without duplicating contracts.
- **Web Module** – Configures Razor/Blazor services, registers the REST client, and maps UI routes. The `Book` page fetches data through `restAPiClient.Books`.

## Configuration Map

| Section | Consumer | Default |
| --- | --- | --- |
| `RestApi:BasePath` | REST delivery module & REST client | `/api` (delivery), `http://localhost:5007/api` (client) |
| `PersistenceMongo:ConnectionString` | Mongo module | `mongodb://localhost:20027` |
| `PersistenceMongo:DatabaseName` | Mongo module | `library_management` |
| `Domain:*` | Domain module | Sample `Test` option (extend/replace as needed) |

Always document new configuration keys in the relevant README + this table.

## Testing Strategy

- **Domain** – Unit tests per use case (pending as the aggregate grows).
- **Mongo Persistence** – xUnit + Testcontainers integration tests assert adapters persist and query correctly.
- **REST Delivery** – xUnit tests validate module registration and endpoint routing using `WebApplicationBuilder`.
- **REST Client** – xUnit tests intercept HTTP calls via custom handlers.
- **Blazor UI** – bUnit tests reside in `.razor` files with `.razor.cs` partials (see `BookPageTests`) so discovery works.

When adding new adapters, mirror them with a `{Project}.Tests` project and describe the coverage in its README.

## UI Considerations

- The Blazor Server app (`LibraryManagement.Web`) doubles as a hybrid host for Blazor WebAssembly by using `.AddInteractiveServerComponents()` and `.AddInteractiveWebAssemblyComponents()`.
- The Web client registers `AddWebClientModule()` to ensure the REST client base address matches the API.
- Component tests should emulate the pattern used by `BookPageTests`: `@inherits TestContext`, register dependencies (e.g., stubbed `IRestAPiClient`) inside the test body, and keep logic inside `.razor` while the partial `.razor.cs` remains empty.

## ADRs & Documentation

Record significant decisions inside `docs/adr/`. Keep the README of every project in sync with the code they describe, and update `docs/project-roadmap.md` whenever milestones move forward.
