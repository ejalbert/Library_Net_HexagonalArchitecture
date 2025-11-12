# LibraryManagement.Application.Tests

Integration test suite that boots the real `LibraryManagement.Application` host via `WebApplicationFactory<ApplicationAssemblyMarker>`. `ApplicationWebApplicationFactory` swaps Mongo persistence ports with an in-memory test double so the composed API + UI can run without external services.

## Covered Scenarios

- Book endpoints: create, fetch, search, and delete requests traverse the REST, domain, and (stubbed) persistence layers.
- Blazor shell + static assets are reachable (`/` and `/app.css`) proving the web module wiring.
- `PersistenceMongoModuleOptions` bind to configuration overrides.

## Commands

```bash
dotnet test tests/LibraryManagement.Application.Tests/LibraryManagement.Application.Tests.csproj
```

Run from the repository root so solution-relative content roots resolve correctly.
