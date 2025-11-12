# LibraryManagement.Persistence.Mongo

MongoDB adapter that implements the outbound ports defined by the domain layer. The current slice stores and queries books and now persists authors (create use case).

## Responsibilities

- Provides `PersistenceMongoModule` to register MongoDB dependencies (client, database, options) through the module bootstrapper.
- Implements `IBookCollection`, `IAuthorCollection`, their entities, and Mapperly-powered conversions between entities and domain models.
- Supplies adapters for `ICreateAuthorPort`, `ICreateNewBookPort`, `IUpdateBookPort`, `IDeleteBookPort`, `IGetSingleBookPort`, and `ISearchBooksPort`.

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
  Domains/Authors/
    AuthorEntity.cs
    AuthorEntityMapper.cs
    AuthorCollection.cs
    Adapters/*.cs
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
- Unit and integration tests for each adapter (authors + books).
- Integration tests that spin up MongoDB 7 with Testcontainers to verify persistence end-to-end.

Ensure Docker is running before executing the integration suite.

## Integration Points

The module registers:

- `MongoClient` (singleton) and scoped `IMongoDatabase`.
- Author + book services (collections, mappers, adapters).

Add additional collections/adapters here as new aggregates move into MongoDB.
