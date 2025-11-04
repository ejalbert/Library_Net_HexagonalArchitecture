# Architecture Guide

This project follows Hexagonal (Ports and Adapters) architecture to decouple business logic from delivery and infrastructure frameworks.

## Layering Strategy

- **Domain Core**
  - Contains aggregates, entities, value objects, domain services, and domain events.
  - Absolutely no framework dependencies.
  - Focus on rich behavioural models and invariants.

- **Application Layer**
  - Implements use-case orchestration.
  - Exposes input ports (interfaces) consumed by adapters.
  - Depends only on the domain and DTO contracts.

- **Adapters**
  - **Inbound (Driving)** adapters translate external requests (HTTP, CLI, messaging) into application commands.
  - **Outbound (Driven)** adapters implement infrastructure concerns (databases, external APIs) matching outbound port interfaces.

## Project Structure (Planned)

```
src/
  YourApp.Domain/
  YourApp.Application/
  YourApp.Infrastructure/
  YourApp.Api/
tests/
  YourApp.Domain.Tests/
  YourApp.Application.Tests/
  YourApp.Infrastructure.Tests/
```

Replace `YourApp` with the chosen solution prefix. Each project should map cleanly to the respective layer, ensuring dependencies flow inwards.

### Project Documentation Conventions

- Every project directory (including adapters and test projects) must include a `README.md` describing its responsibilities, dependencies, and how to run its scripts or commands.
- Seed new project READMEs from `docs/templates/project-readme-template.md` to promote consistent guidance.
- Commands documented inside a project `README.md` must succeed when executed from that project's directory; use relative paths (e.g., `../../docs/...`) when referencing resources outside the folder.

### Solution Organisation

- Group related projects in solution folders (e.g., place `YourApp.Api` and `YourApp.Api.Tests` within a shared `Api` folder) to highlight ownership boundaries and keep `{Project}`/`{Project}.Tests` pairs together.
- Apply the same pattern for adapter groupings (e.g., `Adapters/Infrastructure`, `Adapters/Delivery`) so the solution structure mirrors the architecture diagram while keeping navigation manageable.

## Configuration & DI

- Compose the object graph in the delivery adapter (e.g., `YourApp.Api`).
- Application layer exposes interfaces for outbound dependencies; infrastructure provides concrete implementations registered via dependency injection.

## Data Persistence

- Start with in-memory repositories to support early testing.
- Plan for an EF Core adapter (or alternative) with repositories implementing the application outbound ports.
- Provide containerised dev dependencies (e.g., MongoDB, Redis) via Docker Compose so local environments mirror integration targets.

## Distributed Deployment Considerations

- Compose adapters so each delivery mechanism can be deployed independently (e.g., API, background workers, schedulers) while sharing the same application and domain layers.
- Favour stateless inbound adapters; persist session state or caches via outbound ports so scaling out across multiple nodes remains trivial.
- Keep configuration externalised (environment variables, configuration providers) and injected through the composition root to support multiple hosting targets.
- Document adapter combinations for common deployment topologies in the `docs/` folder as they emerge.

## Scalability Patterns

- Use domain events and application ports to orchestrate asynchronous workflows and eventual consistency without leaking infrastructure details into the domain.
- Apply the outbox pattern (or equivalent) within infrastructure adapters when integrating with message brokers to preserve transactional guarantees.
- Partition repositories by aggregate root boundaries so storage engines can scale independently.
- Treat caching, rate limiting, and resiliency policies (retry, circuit breaker) as decorators around outbound ports to keep them testable and replaceable.

## Cross-Cutting Concerns

- Observability (logging, tracing, metrics) should be implemented as adapters or decorators registered by the composition root.
- Centralise security concerns (authentication, authorisation) within delivery adapters while keeping role/permission checks expressible at the application layer.
- Define clear configuration contracts so secrets and deployment-specific settings can be managed via infrastructure tooling rather than hard-coded defaults.

## Testing Strategy

- Mirror each primary project with a `{Project}.Tests` counterpart using xUnit; adopt bUnit for UI-facing adapters and Moq for mocks/stubs.
- **Domain** – Pure unit tests that verify behaviour and invariants. Every class with business logic should have a focused test class.
- **Application** – Use mocked outbound ports to verify use-case logic, ensuring command/query orchestration follows the defined ports.
- **Adapters** – Provide integration tests for delivery and infrastructure adapters, including external services such as MongoDB, SQL Server, Redis, RabbitMQ, or other system entry points.

## Decision Records

Document significant architectural decisions under `docs/adr/` using Markdown files named `NNNN-title.md`. Use [MADR](https://adr.github.io/madr/) or a similar template.

## Additional Reading

- [Hexagonal Architecture Overview](https://alistair.cockburn.us/hexagonal-architecture/) *(external)*
- [Domain-Driven Design Reference](https://www.domainlanguage.com/ddd/reference/) *(external)*
