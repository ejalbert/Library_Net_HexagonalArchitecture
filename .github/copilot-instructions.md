# Copilot Instructions

These notes prime GitHub Copilot (Chat & IDE) with the context that guides this repository.

## Project Snapshot

- Solution: .NET 9 hexagonal architecture for a library domain.
- Key layers: Domain (core logic), Application (use cases), Infrastructure (adapters), Delivery (API/CLI).
- Tests: xUnit with FluentAssertions; integration coverage for adapters.

## Development Guardrails

- Preserve domain purity: no infrastructure dependencies inside `Library.Domain`.
- Surface domain invariants through rich domain types instead of primitive obsession.
- Keep adapters thin; route requests through application ports.
- Update documentation (`README.md`, `docs/architecture.md`, `docs/adr/`) when behaviour or decisions change.

## Coding Conventions

- C# style: PascalCase for types, camelCase for locals/parameters, suffix interfaces with `I` only when necessary.
- Prefer immutable value objects; use records where it fits the intent.
- Use dependency injection; avoid static singletons.
- Tests should read Given/When/Then.

## Workflow Expectations

- Default branch is `main`; work on feature branches and squash merge.
- Run `dotnet format` and `dotnet test` before opening a PR.
- Capture significant choices as ADRs in `docs/adr/`.
- Maintain AI-friendly TODOs: `TODO(agent): reason` with actionable context.

## Response Guidance (Chat)

When proposing changes:

1. Summarise the intent before showing code.
2. Limit diffs to affected files and explain why.
3. Provide validation steps (`dotnet test`, etc.).
4. Flag uncertainties or follow-up tasks explicitly.

## Non-Goals

- Do not introduce frameworks that tightly couple the domain (e.g., direct EF Core entities in domain layer).
- Avoid generating boilerplate without discussing its necessity.

Keep responses concise, architecture-aware, and aligned with the roadmap in `docs/project-roadmap.md`.
