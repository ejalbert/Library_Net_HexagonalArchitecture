# AGENTS

Playbook for coding assistants working on the Hexagonal Library Management System (.NET 10 preview).

## Snapshot
- Architecture: Hexagonal with module bootstrapper; hosts call `InitializeApplicationModuleConfiguration()` then chain `Add*Module()` / `Use*Module()` to compose Domain, Mongo, Postgres, REST API + client, OpenAI, Blazor Server + WebAssembly. Desktop (WPF) and Semantic Kernel AI adapter are present with READMEs under `src/LibraryManagement.Clients.Desktop` and `src/LibraryManagement.AI.SemanticKernel`.
- Domain: books (create/search/get/update/patch/delete), authors (create/search), AI book suggestions, AI consumption tracking, tenant identification. Outbound ports are implemented by Mongo/Postgres adapters and the OpenAI module.
- Delivery: REST module exposes `/api/v1/books`, `/api/v1/authors`, `/api/v1/ai/book-suggestions` with DTOs mirrored in `LibraryManagement.Api.Rest.Client`. Blazor UI consumes the typed REST client; bUnit tests live in `.razor` files with matching partial `.razor.cs` files for discovery (`tests/LibraryManagement.Web.Tests/Components/*`).
- Persistence: Mongo adapter with Mapperly mappings; Postgres EF Core adapter with migrations and a SaveChanges interceptor that enforces tenant IDs. AI consumption logging lives in Postgres.
- Docs: `docs/architecture.md`, `docs/project-roadmap.md`, `docs/ai-collaboration.md`, ADRs under `docs/adr/`, project READMEs under each `src/` and `tests/` folder. Use `docs/templates/project-readme-template.md` when creating new projects.

## Ways of Working
- Keep domain projects infrastructure-agnostic. Register services/endpoints through module extensions instead of touching hosts directly; update both API and REST client when changing routes/DTOs.
- Update READMEs + docs whenever behaviour changes. Surface open questions as `TODO(agent): description` and maintain checklists for multi-step work.
- Honour multitenancy: REST maps the `tenant_id` claim into `IGetCurrentUserTenantIdUseCase` (defaults to `00000000-0000-0000-0000-000000000001`). The Postgres interceptor overwrites `TenantId` on save—tests must mock tenant IDs per `DbContext`.
- Secrets: never commit API keys. Configure OpenAI via user-secrets or environment variables (`OpenAi__ApiKey`, `OpenAi__Model`).
- Keep REST contracts, Blazor UI, and documentation in sync so server/client/host compose cleanly.

## Environment & Configuration
- Dependencies: Docker (for Testcontainers), compose services in `compose-dev.yaml` (mongo 27017, postgres 5432 + pgAdmin, redis, rabbitmq).
- Key config sections:
  - `RestApi:BasePath` (`/api` default; REST client defaults to `http://localhost:5007/api`).
  - `PersistenceMongo:ConnectionString` / `DatabaseName` (README default `mongodb://localhost:20027` / `library_management`; adjust to compose port 27017 when using the dev stack).
  - `PersistencePostgres:ConnectionString` / `DatabaseName` (defaults to `Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres` / `library_management`).
  - `OpenAi:ApiKey` / `Model` (default `gpt-4.1-nano`).
- Run the composed host: `dotnet run --project src/LibraryManagement.Application/LibraryManagement.Application.csproj` (ensure backing services are up if not using in-memory doubles).

## Validation
- Targeted tests (run from repo root):
  - Domain: `dotnet test tests/LibraryManagement.Domain.Tests/LibraryManagement.Domain.Tests.csproj`
  - REST module: `dotnet test tests/LibraryManagement.Api.Rest.Tests/LibraryManagement.Api.Rest.Tests.csproj`
  - REST client: `dotnet test tests/LibraryManagement.Api.Rest.Client.Tests/LibraryManagement.Api.Rest.Client.Tests.csproj`
  - Mongo adapter (Docker): `dotnet test tests/LibraryManagement.Persistence.Mongo.Tests/LibraryManagement.Persistence.Mongo.Tests.csproj`
  - Postgres adapter (Docker): `dotnet test tests/LibraryManagement.Persistence.Postgres.Tests/LibraryManagement.Persistence.Postgres.Tests.csproj`
  - Blazor UI: `dotnet test tests/LibraryManagement.Web.Tests/LibraryManagement.Web.Tests.csproj`
  - Integrated host (in-memory persistence): `dotnet test tests/LibraryManagement.Application.Tests/LibraryManagement.Application.Tests.csproj`
- Re-run relevant suites whenever touching those areas; keep Testcontainers prerequisites documented.

## Patterns & Pitfalls
- API/UI alignment: extend both `LibraryManagement.Api.Rest` and `LibraryManagement.Api.Rest.Client` together; update Blazor components and desktop client stubs as needed.
- Persistence: use Mapperly mappings; let the Postgres interceptor set `TenantId`. For multitenant tests use separate contexts with distinct tenant mocks—do not set `TenantId` directly.
- Blazor testing rule: each `.razor` test component needs a partial `.razor.cs` file (see root README and `docs/ai-collaboration.md`).
- New projects/modules: scaffold a README from the template, register via module bootstrapper extensions, and add a sibling `{Project}.Tests`.
