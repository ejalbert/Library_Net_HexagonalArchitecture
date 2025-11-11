# LibraryManagement.Persistence.Mongo

MongoDB adapter that implements the outbound ports defined by the domain layer. The current slice stores and queries books.

## Responsibilities

- Provides `PersistenceMongoModule` to register MongoDB dependencies (client, database, options) through the module bootstrapper.
- Implements `IBookCollection`, `BookEntity`, and Mapperly-powered conversions between entities and domain models.
- Supplies adapters for `ICreateNewBookPort`, `IGetSingleBookPort`, and `ISearchBooksPort`.

## Dependencies

- `MongoDB.Driver` / `MongoDB.EntityFrameworkCore` for data access.
- `LibraryManagement.Domain` for the outbound port interfaces and domain models.
- `LibraryManagement.ModuleBootstrapper` to align with the module registration flow.

## Directory Layout

```
LibraryManagement.Persistence.Mongo/
  Abstractions/
    AbstractCollection.cs
    IAbstractCollection.cs
  Domains/Books/
    BookEntity.cs
    BookEntityMapper.cs
    BookCollection.cs
    Adapters/*.cs
  ModuleConfigurations/
    PersistenceMongoModule*.cs
```

## Commands

```bash
# Build the adapter
dotnet build

# Run integration + mapping tests
dotnet test ../../tests/LibraryManagement.Persistence.Mongo.Tests/LibraryManagement.Persistence.Mongo.Tests.csproj
```

## Configuration

`PersistenceMongoModule` binds `PersistenceMongo:ConnectionString` and `DatabaseName`, defaulting to `mongodb://localhost:20027` and `library_management`. Module consumers can override these via configuration or delegate options.

## Tests

`LibraryManagement.Persistence.Mongo.Tests` covers:

- Mapper correctness (`BookEntityMapperTests`).
- Unit tests for each adapter (create, search, get).
- Integration tests that spin up MongoDB 7 with Testcontainers to verify persistence end-to-end.

Ensure Docker is running before executing the integration suite.

## Integration Points

The module registers:

- `MongoClient` (singleton) and scoped `IMongoDatabase`.
- Book-specific services (collection, mapper, adapters).

Add additional collections/adapters here as new aggregates move into MongoDB.
