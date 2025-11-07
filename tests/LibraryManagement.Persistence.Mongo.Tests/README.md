# LibraryManagement.Persistence.Mongo.Tests

## Purpose

- xUnit suite focused on the MongoDB persistence adapter.
- Provides integration coverage for repositories, mapper utilities, and outbound ports backed by Mongo.

## Dependencies

- References `LibraryManagement.Persistence.Mongo` to exercise the real adapters.
- Uses `xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`, and `coverlet.collector`.

## Directory Layout

```
LibraryManagement.Persistence.Mongo.Tests/
  LibraryManagement.Persistence.Mongo.Tests.csproj
  UnitTest1.cs
  README.md
```

## Commands

```bash
# Restore and run persistence tests
dotnet restore
dotnet test
```

## Tests

- Start by covering the `CreateNewBook` adapter and `IBookCollection` logic using an in-memory or containerized Mongo instance.
- Add fixtures that seed sample catalogues so CRUD behaviour can be asserted deterministically.
- Record test data schemas in `docs/adr` when significant modelling choices are made.

## Environment & Configuration

- Use `PersistenceMongo` configuration overrides to point at a local Mongo instance (default `mongodb://localhost:20027`).
- Consider using testcontainers or docker-compose profiles for reproducible integration testing.

## Related Documentation

- `../../docs/architecture.md`
- `../../docs/ai-collaboration.md`
- `../../docs/project-roadmap.md`

## Maintenance Notes

- Replace `UnitTest1` with suites dedicated to each aggregate (books, patrons, loans) as adapters are written.
- Include resilience tests once retry policies or transactions are added.
