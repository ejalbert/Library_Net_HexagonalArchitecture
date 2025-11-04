# Project Roadmap (Draft)

## Milestone 0 – Foundation

- [x] Repository documentation scaffolding
- [ ] Establish .NET solution structure (`YourApp.Domain`, `YourApp.Application`, etc.)
- [ ] Configure continuous integration workflow

## Milestone 1 – Core Domain

- [ ] Model core aggregates, entities, and value objects for the chosen domain
- [ ] Implement domain services to enforce business policies and invariants
- [ ] Add unit tests for core invariants

## Milestone 2 – Application Services

- [ ] Define use-case commands/queries (borrow, return, search)
- [ ] Implement application ports and DTOs
- [ ] Introduce in-memory repositories for testing

## Milestone 3 – API Adapter

- [ ] Scaffold ASP.NET Core Web API
- [ ] Map endpoints to application services
- [ ] Add integration tests and OpenAPI documentation

## Milestone 4 – Persistence Adapter

- [ ] Introduce relational database adapter (EF Core or alternative)
- [ ] Configure migrations and seeding
- [ ] Strengthen integration tests with test containers
- [ ] Add Docker Compose services (e.g., MongoDB, Redis) to support local development and integration testing

## Milestone 5 – Distributed Hosting & Scalability

- [ ] Containerise delivery and infrastructure adapters (Dockerfiles, Compose baseline)
- [ ] Document deployment topology options (single node, multi-node, edge adapters)
- [ ] Introduce messaging or eventing strategy aligned with application ports (e.g., outbox, message broker adapter)
- [ ] Exercise horizontal scaling scenarios in staging environments

## Milestone 6 – Quality & Operations

- [ ] Implement observability (logging, metrics)
- [ ] Add performance and load testing strategy
- [ ] Prepare deployment scripts and release checklist

Roadmap items are aspirational and should be refined as discovery continues. Update this document alongside code changes so all contributors—including AI assistants—share the same direction.
