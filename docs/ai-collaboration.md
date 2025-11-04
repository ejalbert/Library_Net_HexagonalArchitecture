# AI Collaboration Guide

This repository is intended to be co-developed by humans and coding assistants such as GitHub Copilot and Codex CLI agents.

## Shared Principles

- **Single source of truth** – Keep documentation, scripts, and code consistent. Update docs immediately after significant changes.
- **Short feedback loops** – Run tests locally before pushing. Capture expected commands in README or script files so AIs can repeat them.
- **Explicit context** – When creating new modules or patterns, document the intent using docstrings or comments where it aids comprehension.
- **Deterministic tooling** – Prefer idempotent CLI commands and avoid interactive prompts so automations can run unattended.

## Working Agreements

- Use descriptive commit messages and include references to issues or tasks when available.
- Track open questions in `TODO:` comments or GitHub issues; include enough context for AIs to act on them without guesswork.
- Prefer Markdown checklists for multi-step changes so progress is visible to both humans and agents.

## Project Scaffolding Expectations

- When creating a new project (under `src/`, `tests/`, or adapters), always add a `README.md` in that directory describing its scope, dependencies, and how to build/test it.
- Copy `docs/templates/project-readme-template.md` as a starting point when generating project READMEs.
- Ensure every command documented in a project `README.md` works when executed from that directory; use relative paths that walk up to the repository root before referencing other locations.
- Validate example scripts or commands after scaffolding so future agents can rely on them without manual fixes.
- Always scaffold a matching `{Project}.Tests` project that uses xUnit (plus bUnit for UI adapters) and Moq, and ensure classes containing business logic gain corresponding unit tests; add integration tests for adapters that hit external services or serve as system entry points.
- Organise the `.sln` using solution folders so each `{Project}` sits alongside its `{Project}.Tests` counterpart and architecture areas (domain, application, adapters) remain easy to navigate.
- Maintain `docker-compose.yml` (and overrides) to provision development services such as MongoDB and Redis; keep environment variables and documentation aligned with the compose definitions.

## Pull Request Checklist

- [ ] Tests added or updated.
- [ ] Documentation updated (README, architecture notes, ADRs).
- [ ] Code formatted and linted.
- [ ] Breaking changes highlighted in the PR description.

## Prompting Tips for Agents

- Provide the objective, relevant files, and constraints up front.
- Ask for explicit validation steps (tests to run) after code changes.
- Review diffs produced by agents and annotate unexpected modifications.

## Traceability

Maintain a `CHANGELOG.md` once releases start. Until then, include a summary of changes in PR descriptions so the evolution of the project remains understandable.
