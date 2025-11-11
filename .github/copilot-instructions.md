# Copilot Instructions

Prime GitHub Copilot (Chat & IDE) with the context that guides this repository.

## Project Snapshot

- .NET 10 solution built around modules: domain, Mongo persistence, REST delivery, REST client, Blazor Web, workspace bootstrapper.
- Current vertical slice: managing books end-to-end (create/search/get) via REST + Mongo + Blazor.
- Tests rely on xUnit, bUnit (for Blazor), Moq, and Testcontainers for Mongo integration.

## Development Guardrails

- Keep the domain layer framework agnostic; infrastructure adapters implement outbound ports and are composed through `Add*Module()`.
- Update `docs/architecture.md`, READMEs, and ADRs whenever behaviour or dependencies change.
- Maintain parity between the REST API and the REST client package whenever DTOs or routes evolve.
- **Blazor tests**: write component tests as `.razor` files with `.razor.cs` code-behind (see `BookPageTests`) so the runner discovers them.

## Coding Conventions

- C# defaults: PascalCase for types, camelCase for locals/parameters, expression-bodied members when clear.
- Prefer dependency injection over statics; register services through the module bootstrapper.
- Use Mapperly for DTO/entity mapping where practical.

## Workflow Expectations

- Base branch: `main`. Work on feature branches and squash merge.
- Run targeted `dotnet test` (including Mongo Testcontainers) plus `dotnet format` before opening a PR.
- Record significant choices as ADRs in `docs/adr/`.

## Response Guidance

1. Summarise the intent before presenting code.
2. Show only the relevant diffs/files and explain why each change matters.
3. Provide validation steps (tests/commands) tailored to the touched projects.
4. Call out uncertainties or required follow-ups.

## Non-Goals

- Do not introduce infrastructure-specific dependencies into the domain.
- Avoid adding frameworks that bypass the module system or duplicate REST contracts.

Keep responses concise, architecture-aware, and aligned with the roadmap in `docs/project-roadmap.md`.
