# LibraryManagement.Persistence.Postgres.Seeders

Console seeding tool for the Postgres persistence module. It creates the database (if needed) and populates baseline
tenants, authors, and books.

## Responsibilities

- Ensures the Postgres database exists for local development.
- Seeds tenants, authors, and books using EF Core seeding hooks.
- Uses the same `LibraryManagementDbContext` and multitenancy interceptor as the runtime module.

## Dependencies

- `LibraryManagement.Persistence.Postgres` for the EF Core DbContext and interceptors.
- `LibraryManagement.Domain` for tenant ID access.
- Shared architecture guidance: `../../docs/architecture.md`.

## Directory Layout

```
LibraryManagement.Persistence.Postgres.Seeders/
  Program.cs
  Domain/
    Authors/
    Books/
  Infrastructure/
    Tenants/
```

## Commands

```bash
# Build the seeder
dotnet build

# Run the seeder
dotnet run
```

## Tests

- No dedicated tests yet. Add `LibraryManagement.Persistence.Postgres.Seeders.Tests` if seeding logic grows.

## Integration Points

- Uses `LibraryManagementDbContext` and `MultitenantSaveChangesInterceptor` from the Postgres module.
- Seeds data through `SeedTenants`, `SeedAuthors`, and `SeedBooks` extensions.

## Environment & Configuration

- `ConnectionStrings:postgres` (preferred) or `PersistencePostgres:ConnectionString` define the target database.
- Defaults to `Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres` when not configured.

## Related Documentation

- `../../docs/architecture.md`
- `../../docs/adr/0001-multitenancy-enforcement-and-testing.md`

## Maintenance Notes

- TODO(agent): Add validation or idempotency checks if seeding becomes part of CI/CD.
