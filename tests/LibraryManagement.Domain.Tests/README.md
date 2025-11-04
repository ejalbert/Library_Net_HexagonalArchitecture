# LibraryManagement.Domain.Tests

## Purpose

- xUnit test suite covering aggregates, entities, value objects, and policies in `LibraryManagement.Domain`.
- Ensures domain invariants hold without relying on infrastructure concerns.

## Dependencies

- Depends on `LibraryManagement.Domain` via project reference.
- Requires NuGet packages `xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`, and `coverlet.collector` (restored via `dotnet restore`).

## Directory Layout

```
LibraryManagement.Domain.Tests/
  LibraryManagement.Domain.Tests.csproj
  README.md
  UnitTest1.cs
```

## Commands

```bash
# Restore required packages
dotnet restore

# Run all domain tests
dotnet test
```

## Tests

- Add meaningful unit tests per aggregate/policy (loan limits, hold prioritisation, fine calculation, etc.).
- Use data-driven tests where policies require multiple scenarios.

## Integration Points

- Exercises only domain logic; avoid infrastructure dependencies.
- Emit domain events for verification through in-memory handlers when necessary.

## Environment & Configuration

- No environment configuration required. Tests must remain deterministic and self-contained.

## Related Documentation

- `../../docs/architecture.md`
- `../../docs/ai-collaboration.md`
- `../../docs/project-roadmap.md`

## Maintenance Notes

- Replace `UnitTest1` with real test classes mapped to domain types.
