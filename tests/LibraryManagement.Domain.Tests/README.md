# LibraryManagement.Domain.Tests

xUnit project covering the domain use cases for the `Books` aggregate.

## Current Coverage

- Verifies `CreateNewBookService`, `SearchBooksService`, `GetSingleBookService`, `UpdateBookService`, and `DeleteBookService` call their outbound ports with the expected inputs.
- Serves as the foundation for richer behavioural tests (validation, invariants) as the domain grows.

## Commands

```bash
dotnet test
```

Add new tests alongside domain changes and keep this README synchronized with actual coverage.
