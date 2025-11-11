# LibraryManagement.ModuleBootstrapper.AspNetCore

ASP.NET Core-specific companion to the bootstrapper. It lets modules run logic after the `WebApplication` is built (middleware, endpoint mapping, etc.).

## Responsibilities

- Defines `IModuleConfigurator` exposing the running `WebApplication`.
- Implements `UseApplicationModules()` extension that instantiates the configurator and is used by hosts like `LibraryManagement.Application`.

## Commands

```bash
# Build the ASP.NET Core extensions
dotnet build

# Run tests
dotnet test ../../tests/LibraryManagement.ModuleBootstrapper.AspNetCore.Tests/LibraryManagement.ModuleBootstrapper.AspNetCore.Tests.csproj
```

## Tests

`LibraryManagement.ModuleBootstrapper.AspNetCore.Tests` ensures `UseApplicationModules()` returns a configurator bound to the same `WebApplication`. Add regression tests when configurators gain ordering or diagnostics.

## Integration Points

Delivery modules (REST, Web) expose `Use*Module()` methods that extend `IModuleConfigurator`. This package keeps hosts free from module-specific knowledge beyond calling those extensions.
