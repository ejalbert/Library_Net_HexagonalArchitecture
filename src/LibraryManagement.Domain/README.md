# LibraryManagement.Domain

Contains the core domain model and use cases. The current focus is the `Books` aggregate plus the initial `Authors` aggregate (create use case only). Books expose ports for creating, searching, retrieving, updating, and deleting titles, while Authors currently expose the port required to create new authors.

## Responsibilities

- Defines the `Book` entity plus associated commands (`CreateNewBookCommand`, `SearchBooksCommand`, `GetSingleBookCommand`, `UpdateBookCommand`, `DeleteBookCommand`).
- Defines the `Author` entity and the create-author command/port/use-case trio (`CreateAuthorCommand`, `ICreateAuthorPort`, `ICreateAuthorUseCase`).
- Implements use-case services that depend on outbound ports: `ICreateNewBookPort`, `ISearchBooksPort`, `IGetSingleBookPort`, `IUpdateBookPort`, `IDeleteBookPort`, and `ICreateAuthorPort`.
- Provides the `DomainModule` extension so hosts can register domain services via the module bootstrapper and bind options (`DomainModuleOptions`).

## Dependencies

- `Riok.Mapperly` for mapper generation (used by downstream adapters such as Mongo persistence).
- `LibraryManagement.ModuleBootstrapper` for the module registration abstractions.
- No infrastructure dependencies are referenced directly.

## Directory Layout

```
LibraryManagement.Domain/
  Domains/Authors/
    Author.cs
    AuthorServiceRegistration.cs
    Create/...
  Domains/Books/
    Book.cs
    BookServiceRegistration.cs
    Create/...
    Delete/...
    Search/...
    GetSingle/...
    Update/...
  ModuleConfigurations/
    DomainModule.cs
    DomainModuleOptions.cs
```

## Commands

```bash
# Build the domain assembly
dotnet build

# Run the paired tests
dotnet test ../../tests/LibraryManagement.Domain.Tests/LibraryManagement.Domain.Tests.csproj
```

## Tests

`LibraryManagement.Domain.Tests` validates each book use case (create/search/get/update/delete) plus create-author behaviour. Add richer behavioural tests (validation, policy enforcement, identifier generation) as the aggregate grows.

## Configuration

`DomainModuleOptions` currently exposes a sample `Test` property. Replace it with meaningful switches (e.g., feature flags) when domain behaviour needs runtime toggles.

## Integration Points

- Inbound ports implemented by delivery adapters: REST controllers call `ICreateNewBookUseCase`, `ISearchBooksUseCase`, `IUpdateBookUseCase`, etc.
- Outbound ports implemented by `LibraryManagement.Persistence.Mongo` and future infrastructure adapters.

Document any new aggregates or invariants in `docs/architecture.md` as the domain grows.
