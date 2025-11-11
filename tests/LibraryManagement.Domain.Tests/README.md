# LibraryManagement.Domain.Tests

xUnit test project reserved for domain-level unit tests. No fixtures exist yet because the domain currently exposes straightforward pass-through use cases.

## Planned Coverage

- Validation of `CreateNewBookService`, `SearchBooksService`, and `GetSingleBookService` once invariants are introduced.
- Behaviour around identifiers, policies, and errors as additional aggregates are implemented.

## Commands

```bash
# Execute domain tests once scenarios exist
dotnet test
```

Add meaningful tests alongside domain changes and update this README accordingly.
