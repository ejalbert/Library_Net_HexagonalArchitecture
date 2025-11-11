# LibraryManagement.ModuleBootstrapper.Tests

xUnit project that exercises the core bootstrapper abstractions.

## Current Coverage

- `ApplicationBuilderExtensionsTests` verifies that `InitializeApplicationModuleConfiguration()` returns a registrator exposing the same `IServiceCollection`, `IConfiguration`, and environment as the underlying builder.

## Commands

```bash
# Run bootstrapper tests
dotnet test
```

Add regression tests when new functionality is added to the bootstrapper (ordering, diagnostics, validation helpers, etc.).
