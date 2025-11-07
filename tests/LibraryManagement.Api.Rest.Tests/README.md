# LibraryManagement.Api.Rest.Tests

## Purpose

- NUnit test suite covering the REST delivery adapter (`LibraryManagement.Api.Rest`).
- Focuses on HTTP behaviors, routing, OpenAPI exposure, and middleware composition.

## Dependencies

- References `LibraryManagement.Api.Rest` to test the actual module registration and controllers.
- Relies on `NUnit`, `NUnit3TestAdapter`, `NUnit.Analyzers`, `Microsoft.NET.Test.Sdk`, and `coverlet.collector`.

## Directory Layout

```
LibraryManagement.Api.Rest.Tests/
  LibraryManagement.Api.Rest.Tests.csproj
  UnitTest1.cs
  README.md
```

## Commands

```bash
# Restore and run the REST adapter tests
dotnet restore
dotnet test
```

## Tests

- Build out fixture helpers that spin up a minimal ASP.NET Core host configured with `AddRestApiModule` and `UseRestApiModule`.
- Cover success and failure flows for controllers, including model validation, HTTP status codes, and OpenAPI metadata exposure.
- Mock persistence and domain services to keep tests fast until full end-to-end scenarios are required.

## Environment & Configuration

- Tests rely on in-memory configuration (`RestApi` settings) to emulate different base paths or feature flags.
- Avoid binding to a real network port; use the ASP.NET Core test server instead.

## Related Documentation

- `../../docs/architecture.md`
- `../../docs/ai-collaboration.md`
- `../../docs/project-roadmap.md`

## Maintenance Notes

- Replace `UnitTest1` with domain-specific suites (e.g., `BooksEndpointsTests`, `HealthEndpointsTests`).
- Add coverage for error handling middleware as soon as it lands.
