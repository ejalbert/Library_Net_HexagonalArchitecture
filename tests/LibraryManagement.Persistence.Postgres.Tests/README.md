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

## Multitenancy Enforcement

- The Postgres persistence tests must use separate `DbContext` instances, each with a different mock of `IGetCurrentUserTenantIdUseCase`, to insert/query entities for different tenants.
- The EF Core interceptor always overrides `TenantId` on save, so tests must use context-specific mocks to simulate multiple tenants.
- See `docs/adr/0001-multitenancy-enforcement-and-testing.md` for rationale and details.

Add concrete test cases before shipping new Postgres behaviours to keep the EF model and migrations trustworthy.
