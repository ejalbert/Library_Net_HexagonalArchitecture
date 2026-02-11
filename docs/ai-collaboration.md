# AI Collaboration Guide

Humans and coding assistants co-develop this repository. Follow these guidelines to keep the collaboration smooth and accurate.

## Shared Principles

- **Single source of truth** – Update READMEs, docs, and ADRs as soon as behaviour changes so agents inherit the right context.
- **Short feedback loops** – Run the relevant `dotnet test` suites (including Mongo Testcontainers) before pushing.
- **Explicit context** – Document intent via README sections, code comments (sparingly), or ADRs when introducing new modules or workflows.
- **Deterministic tooling** – Prefer idempotent CLI commands with no interactive prompts.

## Working Agreements

- Reference issues or tasks in commit messages when available.
- Use actionable `TODO(agent): description` comments for open questions.
- Maintain Markdown checklists for multi-step work so progress is visible.
- Keep the solution structure mirrored between code and docs; every project (including tests) must have an up-to-date README.
- Annotate REST contracts (DTOs and path parameters) with `System.ComponentModel.Description` to standardize metadata.

## Testing Expectations

- Mirror each project with `{Project}.Tests` using xUnit; infrastructure adapters should include integration coverage via Testcontainers.
- **Blazor component tests must live in `.razor` files with matching `.razor.cs` partials** (see `tests/LibraryManagement.Web.Tests/Components/BookPageTests.*`) to ensure discovery works in bUnit.
- Document required environment prerequisites (Docker, Mongo, etc.) in the associated README whenever a test suite depends on them.

## Project Scaffolding

- When adding a project under `src/` or `tests/`, copy `docs/templates/project-readme-template.md` and fill it out immediately.
- Register new services through the module bootstrapper (`Add*Module()` / `Use*Module()`).
- Keep REST contracts synchronized between the API module and the REST client package.
- Update `compose.yaml` or new deployment manifests when delivery adapters need containers.

## Pull Request Checklist

- [ ] Tests added/updated and executed locally.
- [ ] Documentation updated (READMEs, architecture notes, roadmap, ADRs if needed).
- [ ] Code formatted (`dotnet format`) when touching C# files.
- [ ] Breaking changes called out explicitly.

## Prompting Tips for Agents

- Provide the goal, relevant files, and constraints up front.
- Request explicit validation steps (commands/tests) in agent responses.
- Review generated diffs and annotate unexpected modifications quickly.

## Traceability

Maintain a `CHANGELOG.md` once releases begin. Until then, summarise noteworthy changes inside PR descriptions so history stays understandable.
