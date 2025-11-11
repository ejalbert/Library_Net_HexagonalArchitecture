# Codex CLI Instructions

Guidance for Codex-based agents working inside this repository.

## Repository Snapshot

- Solution targets .NET 10 preview and is structured around modules.
- Modules currently composed by the host (`LibraryManagement.Application`):
  - `LibraryManagement.Domain` – book aggregate with create/search/get use cases.
  - `LibraryManagement.Persistence.Mongo` – MongoDB adapters plus Testcontainers-backed tests.
  - `LibraryManagement.Api.Rest` – minimal API endpoints under `/api/v1/books`.
  - `LibraryManagement.Api.Rest.Client` – shared DTOs + typed HTTP clients.
  - `LibraryManagement.Web` / `.Client` – Blazor Server + WebAssembly front end calling the REST API through the shared client.
- Module bootstrapper libraries (`LibraryManagement.ModuleBootstrapper*`) keep registration consistent across hosts.

Keep these facts in mind when proposing changes so documentation and code stay aligned.

## Operating Principles

1. **Preserve domain purity** – domain projects may only depend on BCL + module abstractions; infrastructure adapters implement outbound ports.
2. **Update docs + READMEs** – every project has a README that must match the current implementation. Re-run this whenever behaviour changes.
3. **Respect module boundaries** – register services via the appropriate `Add*Module()` extension and expose runtime behaviour through `Use*Module()`.
4. **Sync REST contracts** – update both the API module and the REST client when changing DTOs or routes.
5. **Testing parity** – create/update tests next to the code (domain, adapters, clients, UI). Mongo adapter tests rely on Docker/Testcontainers.
6. **Blazor test discovery** – component tests must live in `.razor` files with matching `.razor.cs` partials (see `tests/LibraryManagement.Web.Tests/Components/BookPageTests.*`).

## Command Usage

- Use `dotnet build`/`dotnet test` from the project directory for targeted validation.
- Run `dotnet test tests/LibraryManagement.Persistence.Mongo.Tests/...` with Docker running to execute Mongo integration tests.
- Prefer `rg` for search, `dotnet new` for scaffolding when needed, and keep commands idempotent.

## Deliverable Expectations

- Responses should explain intent before showing code and cite file paths.
- Reference docs such as `docs/architecture.md`, `docs/project-roadmap.md`, and `docs/ai-collaboration.md` when guiding contributors.
- Surface TODOs or follow-ups explicitly so the backlog stays visible.

## Safety & Review

- Never revert user changes without consent; honour existing worktree modifications.
- Confirm with the user before destructive commands (`rm`, `git reset`, etc.).
- Highlight ambiguities or missing requirements rather than guessing.

Following these instructions keeps Codex agents aligned with human contributors and other AI assistants.
