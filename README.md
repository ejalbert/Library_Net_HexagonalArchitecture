# Hexagonal Architecture Template

A .NET 9-based reference implementation of a modular system designed around Hexagonal (Ports and Adapters) architecture principles. The goal of this repository is to build a maintainable, testable, and AI-friendly codebase where core domain logic remains independent from delivery and infrastructure concerns.

## Status

> **Early setup** – Repository scaffolding and documentation only. Source code will follow once architecture decisions are ratified.

## Vision & Scope

- Support rich domain modelling with clear aggregates, value objects, and policies.
- Provide modular application services (REST API first, other adapters later).
- Embrace Hexagonal architecture to keep the domain model isolated, portable, and host-agnostic.
- Enable collaborative development with both human and AI assistants.
- Optimise for distributed deployment scenarios (containers, cloud-native hosting, edge adapters) without coupling core logic to infrastructure choices.

## Architecture Overview

The project will be organised into these primary layers:

- **Domain** – Entities, value objects, domain events, aggregates, and business policies. No external dependencies.
- **Application** – Use cases, input/output ports, and orchestrations. Coordinates domain logic and defines service contracts.
- **Infrastructure** – Persistence, messaging, external integrations implemented via adapters.
- **Delivery Adapters** – API controllers, CLI, scheduled jobs, etc., all conforming to application ports.

See `docs/architecture.md` for more detailed guidance once implementation begins.

## Distributed Hosting & Scalability Strategy

- Keep inbound ports and delivery adapters stateless so they can be scaled horizontally across zones or regions.
- Design outbound adapters (persistence, messaging, search) behind contracts that support swapping in distributed providers without touching the domain or application layers.
- Containerise infrastructure pieces early; start with Docker Compose and evolve towards orchestration platforms such as Kubernetes or Azure Container Apps.
- Leverage domain events and application ports to support asynchronous workflows and eventual consistency patterns (e.g., messaging, background processors).
- Treat observability (logging, tracing, metrics) as adapters so cross-cutting concerns remain consistent across hosting environments.

## Getting Started

1. Install the [.NET 9 SDK](https://dotnet.microsoft.com/download).
2. Clone this repository.
3. Review the documentation in the `docs/` folder to understand conventions before adding code.
4. Provision development services via Docker Compose (MongoDB, Redis, etc.) once the compose file is available.

_Planned tooling_: `dotnet` CLI, Docker Compose, xUnit for tests, FluentAssertions, and Entity Framework Core (or alternative storage adapter) once requirements stabilise.

## Development Workflow

- Prefer driving features by domain use cases.
- Keep adapters thin; business rules live in the domain.
- Comprehensive tests per layer (unit for domain/application, integration for adapters).
- Document decisions in `docs/adr/` (Architecture Decision Records) when non-trivial.

### Branching & Version Control

- `main` holds releasable code.
- Create feature branches for work; squash merge when ready.
- Use conventional commits where possible (`feat:`, `fix:`, etc.).

## Working with AI Coding Agents

AI-assisted development is welcome. Shared guidelines live in `docs/ai-collaboration.md`:

- Keep documentation and code comments up to date so AI agents have accurate context.
- Use TODO notes sparingly and keep them actionable.
- Prefer deterministic scripts/commands that can be executed by automation.

## Contributing

Refer to `CONTRIBUTING.md` for coding standards, testing expectations, and review workflow.

## License

To be determined.
