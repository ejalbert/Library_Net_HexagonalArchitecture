# LibraryManagement.ModuleBootstrapper.AspNetCore

## Purpose

- Extends the core module bootstrapper with ASP.NET Core-specific configurators.
- Provides `IModuleConfigurator` abstractions that expose `WebApplication` so modules can register middleware, endpoints, and pipelines after services are built.
- Acts as the glue between module registration (`IModuleRegistrator`) and module activation (`UseApplicationModules`).

## Dependencies

- References `LibraryManagement.ModuleBootstrapper` for the shared registration contracts.
- Uses the `Microsoft.AspNetCore.App` framework reference to access `WebApplication` and middleware primitives.

## Directory Layout

```
LibraryManagement.ModuleBootstrapper.AspNetCore/
  Extensions/
    WebApplicationExtensions.cs
  ModuleConfigurators/
    IModuleConfiigurator.cs
  LibraryManagement.ModuleBootstrapper.AspNetCore.csproj
  README.md
```

## Commands

```bash
# Restore and build the ASP.NET Core extensions
dotnet restore
dotnet build

# Execute the paired tests
dotnet test ../../tests/TestProject1LibraryManagement.ModuleBootstrapper.AspNetCore.Tests/TestProject1LibraryManagement.ModuleBootstrapper.AspNetCore.Tests.csproj
```

## Tests

- `TestProject1LibraryManagement.ModuleBootstrapper.AspNetCore.Tests` validates middleware wiring and configurator behaviours via xUnit.
- Add integration-style tests that stand up a minimal web host once the configurators expose more logic (ordering, diagnostics, etc.).

## Integration Points

- Hosts call `app.UseApplicationModules()` to retrieve an `IModuleConfigurator` and then chain module-specific extension methods (e.g., `UseRestApiModule`).
- Delivery and infrastructure modules can use this package to contribute to middleware pipelines without depending on the entire ASP.NET Core host project.

## Environment & Configuration

- Relies on `WebApplication.Services` to resolve module options configured during `Add*Module` calls. No additional configuration is stored here.

## Related Documentation

- `../../docs/architecture.md`
- `../../docs/project-roadmap.md`
- `../../docs/ai-collaboration.md`

## Maintenance Notes

- Consider renaming `IModuleConfiigurator` to fix the typo before releasing publicly.
- Future work: support ordered middleware, diagnostics, and health/endpoints that modules can append at runtime.
