# Architecture Guide

This project follows Hexagonal (Ports and Adapters) architecture to decouple the Library Management business logic from delivery and infrastructure frameworks.

## Layering Strategy

- **Domain Core**
  - Contains aggregates, entities, value objects, domain services, and domain events for catalogue items, patrons, loans, holds, fees, and policies.
  - Absolutely no framework dependencies.
  - Focus on rich behavioural models and invariants such as loan limits, reservation queues, and renewal eligibility.

- **Application Layer**
  - Implements circulation, catalogue, and administration use-case orchestration.
  - Exposes input ports (interfaces) consumed by adapters.
  - Depends only on the domain and DTO contracts.

- **Adapters**
  - **Inbound (Driving)** adapters translate external requests (HTTP, CLI, kiosk integrations, messaging) into application commands.
  - **Outbound (Driven)** adapters implement infrastructure concerns (catalogue persistence, patron notification systems, third-party discovery APIs) matching outbound port interfaces.

### Primary Application Host

- The default delivery adapter is an ASP.NET Core project (`LibraryManagement.Api`) that boots every module—catalogue, circulation, patron services, and administration—within a single process.
- Starting all modules together increases baseline CPU and memory usage, but it offers a simplified operational footprint for environments that prefer cohesive deployments (single container/VM, streamlined DevOps pipelines).
- When scaling for peak usage or distributed branch deployments, individual modules can be hosted as stand-alone adapters that still conform to the same application ports, enabling gradual decomposition without reworking domain logic.

## Project Structure (Planned)

```
src/
  LibraryManagement.Domain/
  LibraryManagement.Application/
  LibraryManagement.Infrastructure/
  LibraryManagement.Api/
tests/
  LibraryManagement.Domain.Tests/
  LibraryManagement.Application.Tests/
  LibraryManagement.Infrastructure.Tests/
```

Use `LibraryManagement` (or a closely related prefix) across the solution so library-focused projects map cleanly to the respective layer and dependencies flow inwards.

### Project Documentation Conventions

- Every project directory (including adapters and test projects) must include a `README.md` describing its responsibilities, dependencies, and how to run its scripts or commands; create this README during scaffolding so automation never encounters undocumented folders.
- Seed new project READMEs from `docs/templates/project-readme-template.md` to promote consistent guidance.
- Commands documented inside a project `README.md` must succeed when executed from that project's directory; use relative paths (e.g., `../../docs/...`) when referencing resources outside the folder.

### Solution Organisation

- Group related projects in solution folders (e.g., place `LibraryManagement.Api` and `LibraryManagement.Api.Tests` within a shared `Api` folder) to highlight ownership boundaries and keep `{Project}`/`{Project}.Tests` pairs together.
- Apply the same pattern for adapter groupings (e.g., `Adapters/Infrastructure`, `Adapters/Delivery`) so the solution structure mirrors the architecture diagram while keeping navigation manageable.

## Configuration & DI

- Compose the object graph in the delivery adapter (e.g., `LibraryManagement.Api`).
- Application layer exposes interfaces for outbound dependencies; infrastructure provides concrete implementations registered via dependency injection.

## Data Persistence

- Start with in-memory repositories to support early testing.
- Plan for an EF Core adapter (or alternative) with repositories implementing the application outbound ports for catalogue storage, patron accounts, and transaction history.
- Provide containerised dev dependencies (e.g., PostgreSQL for catalogue data, Redis for caching) via Docker Compose so local environments mirror integration targets.

## Distributed Deployment Considerations

- Compose adapters so each delivery mechanism can be deployed independently (e.g., staff API, patron mobile API, background reminder workers) while sharing the same application and domain layers.
- Favour stateless inbound adapters; persist session state or caches via outbound ports so scaling out across multiple nodes remains trivial.
- Keep configuration externalised (environment variables, configuration providers) and injected through the composition root to support multiple hosting targets.
- Document adapter combinations for common deployment topologies in the `docs/` folder as they emerge.
- Consider the trade-off between the all-in-one ASP.NET Core host (simpler operations, higher resource usage) and module-specific hosts (higher isolation, better scalability). Capture hosting choices in ADRs when the topology changes.

## Scalability Patterns

- Use domain events and application ports to orchestrate asynchronous workflows and eventual consistency (e.g., notifying patrons when holds are ready, escalating overdue fines) without leaking infrastructure details into the domain.
- Apply the outbox pattern (or equivalent) within infrastructure adapters when integrating with message brokers to preserve transactional guarantees.
- Partition repositories by aggregate root boundaries so storage engines can scale independently.
- Treat caching, rate limiting, and resiliency policies (retry, circuit breaker) as decorators around outbound ports to keep them testable and replaceable.

## Cross-Cutting Concerns

- Observability (logging, tracing, metrics) should be implemented as adapters or decorators registered by the composition root so staff can diagnose catalogue or circulation issues quickly.
- Centralise security concerns (authentication, authorisation) within delivery adapters while keeping role/permission checks expressible at the application layer (e.g., staff vs. patron permissions).
- Define clear configuration contracts so secrets and deployment-specific settings (SMTP, SMS, digital lending services) can be managed via infrastructure tooling rather than hard-coded defaults.

## Testing Strategy

- Mirror each primary project with a `{Project}.Tests` counterpart using xUnit; adopt bUnit for UI-facing adapters and Moq for mocks/stubs.
- **Domain** – Pure unit tests that verify behaviour and invariants such as maximum active loans, hold prioritisation, fines, and renewal rules. Every class with business logic should have a focused test class.
- **Application** – Use mocked outbound ports to verify use-case logic (checkout, return, search, patron registration), ensuring command/query orchestration follows the defined ports.
- **Adapters** – Provide integration tests for delivery and infrastructure adapters, including external services such as relational databases, search indexes, message brokers, digital lending providers, or other system entry points.

## Decision Records

Document significant architectural decisions under `docs/adr/` using Markdown files named `NNNN-title.md`. Use [MADR](https://adr.github.io/madr/) or a similar template.

## Additional Reading

- [Hexagonal Architecture Overview](https://alistair.cockburn.us/hexagonal-architecture/) *(external)*
- [Domain-Driven Design Reference](https://www.domainlanguage.com/ddd/reference/) *(external)*
