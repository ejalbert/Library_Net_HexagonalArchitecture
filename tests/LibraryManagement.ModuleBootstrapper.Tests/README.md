# LibraryManagement.ModuleBootstrapper.Tests

## Purpose

- xUnit suite validating the behaviour of the module bootstrapper abstractions.
- Ensures the registrator surfaces `IServiceCollection`, configuration, and environment instances consistently across hosts.

## Dependencies

- References `LibraryManagement.ModuleBootstrapper` to test the production implementation.
- Uses `xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`, and `coverlet.collector`.

## Directory Layout

```
LibraryManagement.ModuleBootstrapper.Tests/
  LibraryManagement.ModuleBootstrapper.Tests.csproj
  UnitTest1.cs
  README.md
```

## Commands

```bash
# Restore and run the bootstrapper tests
dotnet restore
dotnet test
```

## Tests

- Add unit tests for `ModuleRegistrator` to confirm it forwards services, configuration, and environment correctly.
- Cover extension methods such as `InitializeApplicationModuleConfiguration` once they include additional setup logic (ordering, validation, logging).

## Environment & Configuration

- Use fake or in-memory host builders to simulate different environments (Development, Production) without standing up a full app.

## Related Documentation

- `../../docs/architecture.md`
- `../../docs/ai-collaboration.md`
- `../../docs/project-roadmap.md`

## Maintenance Notes

- Replace `UnitTest1` with targeted test fixtures as soon as orchestration logic is implemented.
