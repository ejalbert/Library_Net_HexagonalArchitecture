# LibraryManagement.Domain

## Purpose

- Implements the core library domain model: catalogued items, copies, patrons, loans, reservations, and policies.
- Hosts aggregates, value objects, domain services, and events with no infrastructure dependencies.

## Dependencies

- Depends only on BCL types and other domain-specific packages once introduced.
- Reference shared architectural guidance at `../../docs/architecture.md`.

## Directory Layout

```
LibraryManagement.Domain/
  LibraryManagement.Domain.csproj
  README.md
  Class1.cs
```

## Commands

```bash
# Build
dotnet build

# Run tests for paired project
dotnet test ../../tests/LibraryManagement.Domain.Tests/LibraryManagement.Domain.Tests.csproj
```

## Tests

- Paired with `LibraryManagement.Domain.Tests` (xUnit). Add unit tests for every aggregate, policy, or domain service.
- Tests validate invariants such as loan limits, reservation queues, fine calculation, and availability rules.

## Integration Points

- Exposes domain events consumed by the application layer.
- Consumed by outbound ports defined in `LibraryManagement.Application`.

## Environment & Configuration

- No runtime configuration. Domain logic must remain deterministic and framework agnostic.

## Related Documentation

- `../../docs/architecture.md`
- `../../docs/project-roadmap.md`
- `../../docs/adr/`

## Maintenance Notes

- Replace `Class1` with meaningful domain types before committing functional code.
