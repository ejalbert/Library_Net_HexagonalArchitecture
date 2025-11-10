# TestProject1LibraryManagement.ModuleBootstrapper.AspNetCore.Tests

## Purpose

- xUnit suite for the ASP.NET Core-specific module configurator extensions.
- Validates that `UseApplicationModules` returns an `IModuleConfigurator` that exposes the underlying `WebApplication` for module middleware wiring.

## Dependencies

- References `LibraryManagement.ModuleBootstrapper.AspNetCore` to exercise the actual extensions.
- Uses `xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`, and `coverlet.collector`.

## Directory Layout

```
TestProject1LibraryManagement.ModuleBootstrapper.AspNetCore.Tests/
  TestProject1LibraryManagement.ModuleBootstrapper.AspNetCore.Tests.csproj
  UnitTest1.cs
  README.md
```

## Commands

```bash
# Restore and run the ASP.NET Core bootstrapper tests
dotnet restore
dotnet test
```

## Tests

- Create fake `WebApplication` instances (via `WebApplicationFactory` or minimal hosting) to verify configurator behaviour and middleware chaining.
- Add regression tests around module ordering as soon as the configurator supports advanced scenarios.

## Environment & Configuration

- Keep tests self-contained by using the ASP.NET Core test host; no external services are required.

## Related Documentation

- `../../docs/architecture.md`
- `../../docs/ai-collaboration.md`
- `../../docs/project-roadmap.md`

## Maintenance Notes

- Rename the project folder once a final naming convention is selected.
- Replace `UnitTest1` with targeted suites for middleware registration and option binding.
