# LibraryManagement.Tests.Abstractions

Shared test utility project consumed by the solution's test suites.

## Purpose

- Centralise common fixtures, builders, and fakes so individual test projects stay lean.
- Provide reusable abstractions for HTTP stubs, dependency injection helpers, and domain factories.
- Keep transitive dependencies minimal to avoid bloating every test project's restore surface.

## Guidance

- Add new helpers here before duplicating code across test projects.
- Prefer pure functions or small helper classes over heavyweight test frameworks.
- Document any new utilities in this README so other contributors can discover them.
