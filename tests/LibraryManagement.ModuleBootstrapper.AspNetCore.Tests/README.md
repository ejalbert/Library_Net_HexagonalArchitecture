# LibraryManagement.ModuleBootstrapper.AspNetCore.Tests

xUnit suite focused on the ASP.NET Core-specific configurator.

## Current Coverage

- `WebApplicationExtensionsTests` confirms that `UseApplicationModules()` returns an `IModuleConfigurator` bound to the same `WebApplication` instance, ensuring downstream modules can append middleware/endpoints.

## Commands

```bash
# Run ASP.NET Core bootstrapper tests
dotnet test
```

Expand coverage alongside new configurator features (ordering, diagnostics, health endpoints, etc.).
