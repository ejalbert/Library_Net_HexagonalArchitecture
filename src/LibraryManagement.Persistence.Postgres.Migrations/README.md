# LibraryManagement.Persistence.Postgres.Migrations

EF Core migrations assembly that backs the Postgres persistence module.

## Responsibilities

- Stores generated migrations for `LibraryManagementDbContext`.
- Supplies a minimal host so `dotnet ef` can read connection strings from configuration or environment variables.
- Keeps runtime adapters slim by isolating design-time migration concerns.

## Project Layout

```
LibraryManagement.Persistence.Postgres.Migrations/
  Program.cs                          # Design-time host that wires the DbContext
  Migrations/                         # Generated EF Core migrations (Initial, snapshots)
  LibraryManagement.Persistence.Postgres.Migrations.csproj
```

## Commands

```bash
# Add a migration to the migrations assembly
dotnet ef migrations add <Name> \
  --project src/LibraryManagement.Persistence.Postgres.Migrations/LibraryManagement.Persistence.Postgres.Migrations.csproj \
  --startup-project src/LibraryManagement.Persistence.Postgres.Migrations/LibraryManagement.Persistence.Postgres.Migrations.csproj \
  --output-dir Migrations

# Apply migrations to the configured database
dotnet ef database update \
  --project src/LibraryManagement.Persistence.Postgres.Migrations/LibraryManagement.Persistence.Postgres.Migrations.csproj \
  --startup-project src/LibraryManagement.Persistence.Postgres.Migrations/LibraryManagement.Persistence.Postgres.Migrations.csproj
```

## Configuration

- `ConnectionStrings:postgres` (preferred) or `PersistencePostgres:ConnectionString` – target database for migrations.
- `ConnectionStrings:Default` is still honored for legacy tooling.
- Defaults to `Host=localhost;Port=5432;Database=library_dev;Username=postgres;Password=postgres` when no value is
  supplied.

Keep this project referenced from the Postgres module so runtime calls use the same migrations assembly (
`UseNpgsql(...MigrationsAssembly("LibraryManagement.Persistence.Postgres.Migrations"))`).

```bash
dotnet ef database add AddAuthorEntity -p .\src\LibraryManagement.Persistence.Postgres.Migrations\LibraryManagement.Persistence.Postgres.Migrations.csproj -s .\src\LibraryManagement.Persistence.Postgres.Migrations\LibraryManagement.Persistence.Postgres.Migrations.csproj
```
