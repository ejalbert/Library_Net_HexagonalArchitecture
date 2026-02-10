# AGENTS.md instructions for /Users/etiennejalbert/Library/CloudStorage/OneDrive-Constellio/MAC/sources/Net/Library_Net_HexagonalArchitecture

<INSTRUCTIONS>
## Repository Context
- This is a .NET 10 reference implementation of a modular Library Management System using Hexagonal (Ports & Adapters) architecture.
- Core modules live under `src/` (Domain, Application host, REST API, REST client, Blazor UI, Mongo/Postgres adapters, OpenAI adapter, module bootstrapper).
- Tests mirror projects under `tests/`, typically `{Project}.Tests` with xUnit (and bUnit for Blazor).

## Architecture & Composition Rules
- Domain logic and ports live in `LibraryManagement.Domain`; keep it persistence-agnostic.
- New capabilities should be expressed as domain use cases + outbound ports, then implemented by adapters.
- Register adapters via module extensions (`Add*Module()` / `Use*Module()`) through the module bootstrapper.
- Keep REST client contracts aligned with REST API DTOs; update both together when contracts change.

## AI Collaboration Expectations
- Update READMEs, `docs/architecture.md`, `docs/ai-collaboration.md`, and ADRs immediately when behavior changes.
- Prefer deterministic, non-interactive tooling commands.
- Use explicit `TODO(agent): ...` for open questions.
- For multi-step work, use Markdown checklists so progress is visible.

## Testing & Quality
- Mirror each project with a `{Project}.Tests` project.
- Infrastructure adapters should have integration coverage via Testcontainers.
- **Blazor component tests must be `.razor` files with matching `.razor.cs` partials** for bUnit discovery.
- Run relevant `dotnet test` suites before finishing work and report which were run.
- Run `dotnet format` when touching C# files if formatting changes are expected.

## Multitenancy Enforcement
- Postgres persistence uses an EF Core `SaveChanges` interceptor to enforce tenant IDs.
- The interceptor overwrites `TenantId` on save based on `IGetCurrentUserTenantIdUseCase` (from `tenant_id` claim, defaulting to `00000000-0000-0000-0000-000000000001`).
- Tests must use separate `DbContext` instances with different `IGetCurrentUserTenantIdUseCase` mocks to simulate tenants.
- See `docs/adr/0001-multitenancy-enforcement-and-testing.md` for rationale.

## Documentation Workflow
- Every project (including tests) must have an up-to-date README.
- When adding a new project under `src/` or `tests/`, copy `docs/templates/project-readme-template.md` immediately.
- Update `docs/project-roadmap.md` as milestones move forward.
- Keep `docs/architecture.md` aligned with module changes and new configuration keys.

## Practical Navigation
- Start with `README.md` for solution status and run/test commands.
- For module-specific behavior, read that module’s `README.md` under `src/` or `tests/`.
- For architecture and collaboration norms, read `docs/architecture.md` and `docs/ai-collaboration.md`.

## Task Entrypoints
- Domain changes: `src/LibraryManagement.Domain/README.md`
- REST API changes: `src/LibraryManagement.Api.Rest/README.md`, `src/LibraryManagement.Api.Rest.Client/README.md`
- UI changes: `src/LibraryManagement.Web/README.md`, `src/LibraryManagement.Web/LibraryManagement.Web.Client/README.md`
- Persistence changes: `src/LibraryManagement.Persistence.Mongo/README.md`, `src/LibraryManagement.Persistence.Postgres/README.md`
- AI/OpenAI changes: `src/LibraryManagement.AI.OpenAi/README.md`

## Common Commands (from repo root)
- `dotnet restore`
- `dotnet test tests/LibraryManagement.Domain.Tests/LibraryManagement.Domain.Tests.csproj`
- `dotnet test tests/LibraryManagement.Persistence.Mongo.Tests/LibraryManagement.Persistence.Mongo.Tests.csproj`
- `dotnet test tests/LibraryManagement.Persistence.Postgres.Tests/LibraryManagement.Persistence.Postgres.Tests.csproj`
- `dotnet test tests/LibraryManagement.Api.Rest.Tests/LibraryManagement.Api.Rest.Tests.csproj`
- `dotnet test tests/LibraryManagement.Api.Rest.Client.Tests/LibraryManagement.Api.Rest.Client.Tests.csproj`
- `dotnet test tests/LibraryManagement.Web.Tests/LibraryManagement.Web.Tests.csproj`
- `dotnet test tests/LibraryManagement.Application.Tests/LibraryManagement.Application.Tests.csproj`
- `docker compose -f compose-dev.yaml up -d mongo postgres`
- Testcontainers requires Docker running.

## Contract Change Checklist
- Update REST API DTOs in `LibraryManagement.Api.Rest`.
- Update REST client DTOs in `LibraryManagement.Api.Rest.Client`.
- Update `System.ComponentModel.Description` annotations for OpenAPI metadata.
- Update UI and tests that consume the contract.

## AI Module Guardrails
- OpenAI model name and key are configured under `OpenAi:*`.
- Register new chat tools or adapters via module bootstrapper `Add*Module()` / `Use*Module()`.
- Update `src/LibraryManagement.AI.OpenAi/README.md` whenever AI tools or workflows change.

## Do Not
- Do not introduce infrastructure dependencies into `LibraryManagement.Domain`.
- Do not change REST contracts without updating the REST client package.
- Do not add interactive CLI commands to docs or workflows.

## Documentation Locations
- ADRs: `docs/adr/`
- README template: `docs/templates/project-readme-template.md`
- Roadmap: `docs/project-roadmap.md`

## Skills
A skill is a set of local instructions to follow that is stored in a `SKILL.md` file. Below is the list of skills that can be used. Each entry includes a name, description, and file path so you can open the source for full instructions when using a specific skill.
### Available skills
- skill-creator: Guide for creating effective skills. This skill should be used when users want to create a new skill (or update an existing skill) that extends Codex's capabilities with specialized knowledge, workflows, or tool integrations. (file: /Users/etiennejalbert/.codex/skills/.system/skill-creator/SKILL.md)
- skill-installer: Install Codex skills into $CODEX_HOME/skills from a curated list or a GitHub repo path. Use when a user asks to list installable skills, install a curated skill, or install a skill from another repo (including private repos). (file: /Users/etiennejalbert/.codex/skills/.system/skill-installer/SKILL.md)
### How to use skills
- Discovery: The list above is the skills available in this session (name + description + file path). Skill bodies live on disk at the listed paths.
- Trigger rules: If the user names a skill (with `$SkillName` or plain text) OR the task clearly matches a skill's description shown above, you must use that skill for that turn. Multiple mentions mean use them all. Do not carry skills across turns unless re-mentioned.
- Missing/blocked: If a named skill isn't in the list or the path can't be read, say so briefly and continue with the best fallback.
- How to use a skill (progressive disclosure):
  1) After deciding to use a skill, open its `SKILL.md`. Read only enough to follow the workflow.
  2) When `SKILL.md` references relative paths (e.g., `scripts/foo.py`), resolve them relative to the skill directory listed above first, and only consider other paths if needed.
  3) If `SKILL.md` points to extra folders such as `references/`, load only the specific files needed for the request; don't bulk-load everything.
  4) If `scripts/` exist, prefer running or patching them instead of retyping large code blocks.
  5) If `assets/` or templates exist, reuse them instead of recreating from scratch.
- Coordination and sequencing:
  - If multiple skills apply, choose the minimal set that covers the request and state the order you'll use them.
  - Announce which skill(s) you're using and why (one short line). If you skip an obvious skill, say why.
- Context hygiene:
  - Keep context small: summarize long sections instead of pasting them; only load extra files when needed.
  - Avoid deep reference-chasing: prefer opening only files directly linked from `SKILL.md` unless you're blocked.
  - When variants exist (frameworks, providers, domains), pick only the relevant reference file(s) and note that choice.
- Safety and fallback: If a skill can't be applied cleanly (missing files, unclear instructions), state the issue, pick the next-best approach, and continue.
</INSTRUCTIONS>
