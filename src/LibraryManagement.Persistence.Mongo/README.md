# LibraryManagement.Persistence.Mongo

## Purpose

- MongoDB persistence adapter for the library domain.
- Implements repository-style abstractions for catalogued books (and future aggregates) plus the outbound ports they require.
- Provides a module configuration entry point so Mongo services can be registered consistently via the bootstrapper.

## Dependencies

- References `LibraryManagement.Domain` to satisfy outbound port interfaces (e.g., `ICreateNewBookPort`).
- Uses `MongoDB.Driver` and `MongoDB.EntityFrameworkCore` to interact with MongoDB clusters.
- Relies on `LibraryManagement.ModuleBootstrapper` through extension methods invoked by consuming hosts.

## Directory Layout

```
LibraryManagement.Persistence.Mongo/
  Abstractions/
  Domains/
    Books/
  ModuleConfigurations/
    PersistenceMongoModule.cs
    PersistenceMongoModuleEnvConfiguration.cs
    PersistenceMongoModuleOptions.cs
  LibraryManagement.Persistence.Mongo.csproj
  README.md
```

## Commands

```bash
# Restore and build the MongoDB adapter
dotnet restore
dotnet build

# Run the paired persistence tests
dotnet test ../../tests/LibraryManagement.Persistence.Mongo.Tests/LibraryManagement.Persistence.Mongo.Tests.csproj
```

## Tests

- `LibraryManagement.Persistence.Mongo.Tests` will house xUnit-based integration tests that boot Mongo (local or containerised) and validate adapters end-to-end.
- Add contract tests for each outbound port to ensure mapping consistency between domain entities and Mongo collections.

## Integration Points

- Call `builder.InitializeApplicationModuleConfiguration().AddPersistenceMongoModule()` to register Mongo services within any host.
- Provides DI registrations for `MongoClient`, `IMongoDatabase`, mapper types, and adapter implementations (e.g., `CreateNewBookAdapter`).
- Works alongside other infrastructure modules through the shared bootstrapper contracts.

## Environment & Configuration

- Reads the `PersistenceMongo` configuration section into `PersistenceMongoModuleOptions` with defaults:
  - `ConnectionString`: `mongodb://localhost:20027`
  - `DatabaseName`: `library_management`
- Override via environment variables (`PersistenceMongo__ConnectionString`, `PersistenceMongo__DatabaseName`) or appsettings files per environment.

## Related Documentation

- `../../docs/architecture.md`
- `../../docs/project-roadmap.md`
- `../../docs/ai-collaboration.md`
- `../../docs/adr/` when persistence-specific decisions are recorded.

## Maintenance Notes

- Flesh out the `Domains` folder with concrete collections (Patrons, Loans, Reservations) as those aggregates mature.
- Revisit connection management and resilience policies (retry, health checks) once operational requirements are defined.
