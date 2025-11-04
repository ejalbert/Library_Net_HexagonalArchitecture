# Hexagonal Library Management System

A .NET 9-based reference implementation of a modular Library Management System designed around Hexagonal (Ports and Adapters) architecture principles. The goal of this repository is to model core library workflows—cataloguing, circulation, patron services, and policy enforcement—while keeping the domain logic independent from delivery and infrastructure concerns.

## Status

> **Early setup** – Repository scaffolding and documentation only. Library domain modelling and adapters will follow once architecture decisions are ratified.

## Vision & Scope

- Support rich domain modelling for library assets (books, media, digital subscriptions), patrons, and circulation policies.
- Provide modular application services for catalogue search, checkouts, returns, reservations, and administrative workflows (REST API first, other adapters later).
- Embrace Hexagonal architecture to keep the library domain model isolated, portable, and host-agnostic.
- Enable collaborative development with both human and AI assistants so library-specific knowledge stays accurate and shared.
- Optimise for distributed deployment scenarios (branch libraries, self-service kiosks, cloud-hosted catalogues) without coupling core logic to infrastructure choices.

## Architecture Overview

The project will be organised into these primary layers:

- **Domain** – Entities, value objects, domain events, aggregates, and library policies (e.g., loan rules, hold queues). Contains the behaviour for assets, patrons, and circulation with no external dependencies.
- **Application** – Use cases, input/output ports, and orchestrations. Coordinates domain logic for scenarios like `CheckOutItem`, `PlaceHold`, and `RegisterPatron` and defines service contracts.
- **Infrastructure** – Persistence, messaging, search indexing, and external integrations (e.g., MARC import, identity providers) implemented via adapters.
- **Delivery Adapters** – API controllers, CLI tools, scheduled jobs, or future interfaces (self-check stations, staff dashboards) that invoke application ports. The primary application project is an ASP.NET Core host that boots every module together for cohesive deployments.

The default host loads catalogue, circulation, patron, and administration modules in a single process. This all-in-one startup consumes more CPU and memory, but it simplifies environments that favour a consolidated deployment over microservices. Individual modules can still be extracted into stand-alone hosts when a distributed topology or peak load mitigation requires additional scalability.

See `docs/architecture.md` for more detailed guidance once implementation begins.

## Distributed Hosting & Scalability Strategy

- Keep inbound ports and delivery adapters stateless so the catalogue API, circulation desks, and staff portals can scale across multiple library branches.
- Design outbound adapters (catalogue persistence, patron notification channels, discovery/search engines) behind contracts that support swapping providers without touching the domain or application layers.
- Containerise infrastructure pieces early; start with Docker Compose and evolve towards orchestration platforms such as Kubernetes or Azure Container Apps to support multi-branch deployments.
- Leverage domain events and application ports to support asynchronous workflows such as waitlist promotion, overdue reminders, and inter-library loans.
- Balance resource consumption by running the all-in-one ASP.NET Core host where operational simplicity is paramount, and scale out by running selected modules as stand-alone services in distributed environments experiencing peak usage.
- Treat observability (logging, tracing, metrics) as adapters so operational insights remain consistent across hosting environments.

## Getting Started

1. Install the [.NET 9 SDK](https://dotnet.microsoft.com/download).
2. Clone this repository.
3. Review the documentation in the `docs/` folder to understand the library domain conventions before adding code.
4. Provision development services via Docker Compose (catalogue database, cache, messaging) once the compose file is available.

_Planned tooling_: `dotnet` CLI, Docker Compose, xUnit for tests, FluentAssertions, and Entity Framework Core (or alternative storage adapter) once requirements stabilise.

## Development Workflow

- Prefer driving features by domain use cases (e.g., “As a patron, I can renew a borrowed item”).
- Keep adapters thin; business rules (loan limits, fine calculations, inventory rules) live in the domain.
- Provide comprehensive tests per layer (unit for domain/application, integration for adapters) that capture circulation and catalogue scenarios.
- Document decisions in `docs/adr/` (Architecture Decision Records) when non-trivial, especially when they impact library policies or integrations.

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
