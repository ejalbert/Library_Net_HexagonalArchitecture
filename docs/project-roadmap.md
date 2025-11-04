# Project Roadmap (Draft)

## Milestone 0 – Foundation

- [x] Repository documentation scaffolding
- [ ] Establish .NET solution structure (`LibraryManagement.Domain`, `LibraryManagement.Application`, etc.) with paired `{Project}.Tests` projects and READMEs created during scaffolding
- [ ] Configure continuous integration workflow

## Milestone 1 – Core Domain

- [ ] Model core aggregates, entities, and value objects for library assets, patrons, loans, reservations, and fines
- [ ] Implement domain services to enforce circulation policies (loan periods, renewal limits, hold queues)
- [ ] Add unit tests for core invariants including availability rules and patron eligibility checks

## Milestone 2 – Application Services

- [ ] Define use-case commands/queries (borrow, return, search, manage patrons)
- [ ] Implement application ports and DTOs for circulation, catalogue management, and patron account workflows
- [ ] Introduce in-memory repositories for testing policy enforcement without infrastructure dependencies

## Milestone 3 – API Adapter

- [ ] Scaffold an ASP.NET Core host that runs every module (catalogue, circulation, patrons, administration) within a single process
- [ ] Map endpoints to application services for catalogue search, checkout, renewals, holds, and patron management
- [ ] Add integration tests and OpenAPI documentation describing library workflows, noting the resource trade-offs of the all-in-one host

## Milestone 4 – Persistence Adapter

- [ ] Introduce relational database adapter (EF Core or alternative) capturing catalogue items, copies, patrons, and circulation history
- [ ] Configure migrations and seeding for sample branches and inventory
- [ ] Strengthen integration tests with test containers
- [ ] Add Docker Compose services (e.g., PostgreSQL, Redis) to support local development and integration testing

## Milestone 5 – Distributed Hosting & Scalability

- [ ] Containerise delivery and infrastructure adapters (Dockerfiles, Compose baseline)
- [ ] Document deployment topology options (single branch, multi-branch, public catalogue, staff intranet)
- [ ] Introduce messaging or eventing strategy aligned with application ports (e.g., outbox, message broker adapter) for notifications and waitlist management
- [ ] Exercise horizontal scaling scenarios in staging environments, including branch-specific load

## Milestone 6 – Quality & Operations

- [ ] Implement observability (logging, metrics) that surfaces catalogue or circulation bottlenecks
- [ ] Add performance and load testing strategy covering peak patron activity
- [ ] Prepare deployment scripts and release checklist for library operations teams

Roadmap items are aspirational and should be refined as discovery continues. Update this document alongside code changes so all contributors—including AI assistants—share the same direction.
