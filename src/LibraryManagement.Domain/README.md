# LibraryManagement.Domain

Contains the core domain model and use cases. The current focus is the `Books` aggregate, which exposes ports for creating, searching, and retrieving titles.

## Responsibilities

- Defines the `Book` entity plus associated commands (`CreateNewBookCommand`, `SearchBooksCommand`, `GetSingleBookCommand`, `DeleteBookCommand`).
- Implements use-case services that depend on outbound ports: `ICreateNewBookPort`, `ISearchBooksPort`, `IGetSingleBookPort`, and `IDeleteBookPort`.
- Provides the `DomainModule` extension so hosts can register domain services via the module bootstrapper and bind options (`DomainModuleOptions`).

## Dependencies

- `Riok.Mapperly` for mapper generation (used by downstream adapters such as Mongo persistence).
- `LibraryManagement.ModuleBootstrapper` for the module registration abstractions.
- No infrastructure dependencies are referenced directly.

## Directory Layout

```
LibraryManagement.Domain/
  Domains/Books/
    Book.cs
    BookServiceRegistration.cs
    Create/...
    Delete/...
    Search/...
    GetSingle/...
  ModuleConfigurations/
    DomainModule.cs
    DomainModuleOptions.cs
```

## Commands

```bash
# Build the domain assembly
dotnet build

# Run the paired tests (currently a placeholder project)
dotnet test ../../tests/LibraryManagement.Domain.Tests/LibraryManagement.Domain.Tests.csproj
```

## Tests

`LibraryManagement.Domain.Tests` has not been populated yet. Add unit tests for each use case before expanding the aggregate (e.g., validation, policy enforcement, identifier generation).

## Configuration

`DomainModuleOptions` currently exposes a sample `Test` property. Replace it with meaningful switches (e.g., feature flags) when domain behaviour needs runtime toggles.

## Integration Points

- Inbound ports implemented by delivery adapters: REST controllers call `ICreateNewBookUseCase`, `ISearchBooksUseCase`, etc.
- Outbound ports implemented by `LibraryManagement.Persistence.Mongo` and future infrastructure adapters.

Document any new aggregates or invariants in `docs/architecture.md` as the domain grows.
