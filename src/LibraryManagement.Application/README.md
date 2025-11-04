# LibraryManagement.Application

## Purpose

- ASP.NET Core host that boots catalogue, circulation, patron services, and administration modules in a single process.
- Provides REST endpoints and shared middleware for branch, staff, and patron-facing integrations.

## Dependencies

- Depends on ASP.NET Core runtime packages referenced in the project file.
- Coordinates domain logic via `LibraryManagement.Domain` (project reference to be added once aggregates exist).
- Architecture guidance: `../../docs/architecture.md`.

## Directory Layout

```
LibraryManagement.Application/
  LibraryManagement.Application.csproj
  Program.cs
  appsettings.json
  appsettings.Development.json
  Properties/
  README.md
```

## Commands

```bash
# Restore (requires NuGet connectivity)
dotnet restore

# Build
dotnet build

# Run the web host
dotnet run

# Execute paired tests
dotnet test ../../tests/LibraryManagement.Application.Tests/LibraryManagement.Application.Tests.csproj
```

## Tests

- Paired with `LibraryManagement.Application.Tests` (xUnit).
- Add integration tests for HTTP endpoints, module bootstrapping, and adapter wiring.
- Include component/integration tests for module-specific behaviours as they emerge.

## Integration Points

- Hosts inbound HTTP ports; maps to application services that orchestrate domain logic.
- Will consume outbound adapters (persistence, messaging) through dependency injection when implemented.

## Environment & Configuration

- Default configuration stored in `appsettings.json`; environment overrides in `appsettings.Development.json`.
- Express deployment configuration via environment variables or additional `appsettings.{Environment}.json` files.

## Related Documentation

- `../../docs/architecture.md`
- `../../docs/project-roadmap.md`
- `../../docs/ai-collaboration.md`

## Maintenance Notes

- Remove the template weather forecast endpoint and replace it with module-specific endpoints during initial implementation.
