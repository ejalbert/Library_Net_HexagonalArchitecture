# Project Roadmap

This roadmap tracks milestones for the book-focused vertical slice and highlights upcoming work for additional domains (patrons, circulation, policies).

## Milestone 0 – Foundation ✅

- [x] Repository + documentation scaffolding.
- [x] Establish .NET solution with module bootstrapper, domain, persistence, delivery, and test projects.
- [x] Compose modules inside an ASP.NET Core host.

## Milestone 1 – Book Domain (In Progress)

- [x] Model the `Book` aggregate with create/search/get/update/delete use cases.
- [x] Expose outbound ports for persistence adapters.
- [ ] Add validation/invariants (e.g., duplicate detection, title rules).
- [ ] Add domain unit tests that cover behaviours beyond pass-through commands.

## Milestone 2 – Persistence Adapter (Book Slice) ✅

- [x] Implement MongoDB adapters for the book ports.
- [x] Provide Mapperly mappings and typed collections.
- [x] Add Testcontainers-based integration tests plus unit tests for mappers/adapters.

## Milestone 3 – REST Delivery ✅

- [x] Map `/api/v1/books` endpoints via the REST module.
- [x] Publish OpenAPI in Development and expose typed controllers.
- [x] Add module tests for option binding, DI registrations, and routing.

## Milestone 4 – Client & UI (In Progress)

- [x] Ship `IBooksClient` in the REST client package with HTTP helpers.
- [x] Register the client inside the Blazor Web module and fetch books for the `Book` page.
- [x] Cover the Blazor component with bUnit tests.
- [ ] Replace boilerplate components (Counter/Weather) with real pages.
- [ ] Add CRUD flows (create/edit book) through the UI.

## Milestone 5 – Additional Domains (Planned)

- [ ] Model patrons, circulation policies, and reservations.
- [ ] Extend persistence adapters + REST endpoints for those domains.
- [ ] Capture decisions as ADRs when introducing new infrastructure or workflows.

## Milestone 6 – Operations & Quality (Planned)

- [ ] Add CI pipeline (restore/build/test, including Mongo Testcontainers).
- [ ] Containerise the integrated host and publish images via Compose/Kubernetes manifests.
- [ ] Introduce observability (logging, tracing, metrics) as decorators around ports/adapters.

Revisit this roadmap whenever a milestone is achieved or priorities change so contributors (humans and AI) stay aligned.
