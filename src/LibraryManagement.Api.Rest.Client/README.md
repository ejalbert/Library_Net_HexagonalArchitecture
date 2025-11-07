# LibraryManagement.Api.Rest.Client

## Purpose

- Provides shared REST client contracts (DTOs, request builders, typed clients) that mirror the server-side API.
- Enables other delivery adapters or external services to consume the Library Management REST API without duplicating models.
- Serves as the single source of truth for serialization rules and API-specific constants.

## Dependencies

- Targets .NET 9.0 with implicit usings and nullable reference types enabled.
- Intentionally avoids runtime dependencies today so it can be referenced by any host or test project.
- Add HTTP or serialization packages (e.g., `System.Net.Http.Json`, `Refit`) as clients evolve.

## Directory Layout

```
LibraryManagement.Api.Rest.Client/
  Class1.cs
  LibraryManagement.Api.Rest.Client.csproj
  README.md
```

## Commands

```bash
# Restore and build the client contracts
dotnet restore
dotnet build

# Run the paired contract tests
dotnet test ../../tests/LibraryManagement.Api.Rest.Client.Tests/LibraryManagement.Api.Rest.Client.Tests.csproj
```

## Tests

- `LibraryManagement.Api.Rest.Client.Tests` exercises serialization, contract shape, and helper clients.
- Add snapshot or contract tests any time breaking changes to public DTOs are considered.

## Integration Points

- Referenced by `LibraryManagement.Api.Rest` (server) to avoid drift between server DTOs and published contracts.
- Additional consumers can reference this package for compile-time safety when calling the API.

## Environment & Configuration

- No runtime configuration. Keep the assembly free of environment-specific branching to remain reusable.

## Related Documentation

- `../../docs/architecture.md`
- `../../docs/project-roadmap.md`
- `../../docs/ai-collaboration.md`

## Maintenance Notes

- Replace `Class1` with cohesive namespaces for each API area (Catalogue, Circulation, Patrons, etc.).
- Maintain semantic versioning when the package is eventually published to an artefact feed.
