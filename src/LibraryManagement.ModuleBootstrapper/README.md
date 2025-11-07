# LibraryManagement.ModuleBootstrapper

## Purpose

- Core infrastructure for composing application modules in a host-agnostic way.
- Provides `IModuleRegistrator` abstractions that expose `IServiceCollection`, configuration, and hosting metadata to each module.
- Supplies helper extensions (e.g., `InitializeApplicationModuleConfiguration`) to streamline startup inside console or ASP.NET Core hosts.

## Dependencies

- Targets .NET 9 with implicit usings and nullable reference types enabled.
- Depends on `Microsoft.Extensions.Configuration.Binder` for options binding and `Microsoft.Extensions.Hosting.Abstractions` for host metadata.
- Referenced by domain, persistence, and delivery adapters to share the same module wiring contract.

## Directory Layout

```
LibraryManagement.ModuleBootstrapper/
  Extensions/
    ApplicationBuilderExtensions.cs
  ModuleRegistrators/
    IModuleRegistrator.cs
    ModuleRegistrator.cs
  LibraryManagement.ModuleBootstrapper.csproj
  README.md
```

## Commands

```bash
# Restore and build the bootstrapper library
dotnet restore
dotnet build

# Run paired unit tests
dotnet test ../../tests/LibraryManagement.ModuleBootstrapper.Tests/LibraryManagement.ModuleBootstrapper.Tests.csproj
```

## Tests

- `LibraryManagement.ModuleBootstrapper.Tests` exercises the registrator contract and ensures service/configuration accessors behave predictably.
- Expand coverage as lifecycle hooks (ordering, conditional registration, diagnostics) are introduced.

## Integration Points

- Called from `LibraryManagement.Application` (and future hosts) to expose builder context to modules.
- Consumed by persistence, domain, and delivery modules to register services with zero knowledge of the host type beyond `IHostApplicationBuilder`.

## Environment & Configuration

- Does not define configuration of its own but exposes `IConfigurationManager` and `IHostEnvironment` so modules can bind their sections.

## Related Documentation

- `../../docs/architecture.md`
- `../../docs/project-roadmap.md`
- `../../docs/ai-collaboration.md`

## Maintenance Notes

- Consider adding diagnostics (logging of module registration) and validation helpers once modules grow.
- Keep the API minimal to avoid leaking ASP.NET Core-specific concepts into non-web hosts.
