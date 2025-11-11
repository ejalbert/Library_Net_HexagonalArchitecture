# LibraryManagement.ModuleBootstrapper

Lightweight infrastructure for wiring modules into any `IHostApplicationBuilder`. Hosts call `InitializeApplicationModuleConfiguration()` to obtain an `IModuleRegistrator`, and modules add their registrations through extension methods.

## Responsibilities

- Defines `IModuleRegistrator` and `ModuleRegistrator<TBuilder>` exposing `IServiceCollection`, `IConfigurationManager`, and `IHostEnvironment`.
- Supplies `ApplicationBuilderExtensions.InitializeApplicationModuleConfiguration()` used by every host in the solution.
- Serves as the common dependency for domain, persistence, and delivery modules so they can share the same bootstrapping experience.

## Commands

```bash
# Build the bootstrapper library
dotnet build

# Run associated tests
dotnet test ../../tests/LibraryManagement.ModuleBootstrapper.Tests/LibraryManagement.ModuleBootstrapper.Tests.csproj
```

## Tests

`LibraryManagement.ModuleBootstrapper.Tests` verifies the registrator correctly surfaces the builder services/configuration/environment. Expand coverage as ordering/diagnostics features are added.

## Integration Points

Any host (console, worker, ASP.NET Core) can call the extension to start module registration. Modules should remain thin wrappers that only depend on the abstractions defined here.

Keep this README updated if module lifecycle management grows beyond simple service registration.
