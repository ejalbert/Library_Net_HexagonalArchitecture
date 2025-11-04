# Contributing Guidelines

Thanks for helping build the Hexagonal Library Management System! This guide explains how to get started and the expectations for contributions aimed at catalogue, circulation, and patron-focused features.

## Prerequisites

- .NET 9 SDK
- Git
- Recommended: IDE with C# support (JetBrains Rider, Visual Studio, or VS Code with C# Dev Kit)

## Workflow

1. Fork or clone the repository and create a feature branch (`git checkout -b feat/my-feature`).
2. Sync with `main` frequently (`git pull --rebase origin main`).
3. Keep commits focused and descriptive (use conventional commit prefixes when possible).
4. Open a pull request against `main` once the change is ready.

## Coding Standards

- Follow idiomatic C# conventions (PascalCase for types, camelCase for locals).
- Keep library domain entities (books, copies, patrons, reservations) persistence-agnostic.
- Minimise static state; prefer dependency injection.
- Write XML doc comments only when it clarifies non-obvious logic, such as circulation policies or fine calculations.

## Testing

- Every project must have a sibling `{Project}.Tests` project using xUnit (and bUnit for UI components when applicable) with Moq available for test doubles.
- Classes containing business logic—such as loan rule evaluators, cataloguing workflows, or notification schedulers—require dedicated test classes; lightweight wrappers or DTOs without logic may omit tests.
- Projects that integrate with external services (integrated library systems, email/SMS gateways, discovery layers) or act as system entry points must include integration tests validating those boundaries.
- Add or update tests for all new behaviour and ensure `dotnet test` passes locally before pushing.

## Documentation

- Update `README.md`, `docs/architecture.md`, and relevant ADRs when behaviour or decisions change.
- Add new ADRs under `docs/adr/` for significant design choices.
- Ensure every project (e.g., each folder under `src/` or `tests/`) includes its own `README.md` that explains purpose, build/test commands, and adapter responsibilities.
- Start new project READMEs by copying `docs/templates/project-readme-template.md` and adapting it to the project.
- Author commands in any README so they execute successfully from the README's directory; use relative paths that traverse to the repository root (`../..`) before referencing other folders.
- Use solution folders in the `.sln` to keep `{Project}` and `{Project}.Tests` adjacent and to reflect architectural boundaries (Domain, Application, Adapters, etc.).

## Pull Requests

- Link related issues.
- Provide a summary of the change, tests performed, and any follow-up work.
- Expect code review; please respond to feedback promptly.

## Code of Conduct

Be respectful and inclusive. Treat collaborators—human or AI—fairly and communicate professionally. Report issues privately to the maintainers if needed.
