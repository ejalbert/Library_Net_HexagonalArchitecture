# LibraryManagement.Api.Rest.Client.Tests

## Purpose

- xUnit test suite that protects the public REST client contracts from accidental breaking changes.
- Exercises serialization helpers, typed HTTP clients, and DTO-level validation rules.

## Dependencies

- References `LibraryManagement.Api.Rest.Client` to test the same contracts consumed by external callers.
- Uses `xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`, and `coverlet.collector` for execution and coverage.

## Directory Layout

```
LibraryManagement.Api.Rest.Client.Tests/
  LibraryManagement.Api.Rest.Client.Tests.csproj
  UnitTest1.cs
  README.md
```

## Commands

```bash
# Restore and run the client contract tests
dotnet restore
dotnet test
```

## Tests

- Add scenarios for every DTO or helper once implemented (serialization, optional fields, enums).
- Validate backward compatibility by loading sample payloads exported from the API.

## Environment & Configuration

- No external services are required. Use in-memory JSON payloads or HTTP handlers for deterministic tests.

## Related Documentation

- `../../docs/architecture.md`
- `../../docs/ai-collaboration.md`
- `../../docs/project-roadmap.md`

## Maintenance Notes

- Replace `UnitTest1` with focused test classes such as `BooksClientTests` or `CheckoutContractsTests` as soon as implementations exist.
