# Codex CLI Instructions

This file guides Codex-based agents working in the repository via the Codex CLI harness.

## Repository Context

- Building a digital library system using .NET 9 with hexagonal architecture.
- Layer boundaries: Domain (pure), Application (use cases), Infrastructure (adapters), Delivery (API/CLI).
- Documentation sources: `README.md`, `docs/architecture.md`, `docs/ai-collaboration.md`, `docs/project-roadmap.md`.

## Operating Principles

1. **Keep domain isolated** – no infrastructure references within domain projects.
2. **Prefer clarity over code generation** – explain rationale before writing files.
3. **Sync docs** – update architecture notes, ADRs, and roadmap when decisions or status change.
4. **Use plans** – outline multi-step work before making changes (skip only for trivial edits).
5. **Respect existing changes** – never revert user modifications unintentionally.

## Command Usage

- Use `dotnet format` and `dotnet test` to validate changes when relevant.
- Prefer `rg` for searching and `dotnet new` for scaffolding when needed.
- Keep commands idempotent; avoid interactive prompts.

## Deliverable Style

- Responses should be concise, with bullet summaries of changes and verification steps.
- Reference files with inline paths (`path/to/file:line`).
- Offer next steps when natural (tests, ADR updates, deployment checks).

## Testing & Quality

- Add or update tests alongside new behaviour.
- For integration work, coordinate adapters via application ports and document assumptions.

## Safety Checks

- Confirm destructive commands with the user before running (`rm`, `git reset`, etc.).
- Highlight ambiguities, missing requirements, or potential regressions.

Following these guidelines keeps Codex agents aligned with human contributors and other AI assistants.
