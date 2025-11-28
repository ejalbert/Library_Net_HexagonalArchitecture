# LibraryManagement.Persistence.Postgres.Tests

Integration-style tests for the Postgres persistence module using real Postgres via Testcontainers.

## Purpose

- Exercise EF Core adapters (book create/delete flows) against Postgres so database-specific behaviour is covered.
- Validate mappings between domain models and `BookEntity`/`BookKeywordEntity`.
- Provide a home for future regression tests as more persistence ports are implemented.

## Running Locally

```bash
# Requires Docker to start a disposable Postgres container
dotnet test tests/LibraryManagement.Persistence.Postgres.Tests/LibraryManagement.Persistence.Postgres.Tests.csproj
```

Add concrete test cases before shipping new Postgres behaviours to keep the EF model and migrations trustworthy.
