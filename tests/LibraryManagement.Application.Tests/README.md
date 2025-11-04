# LibraryManagement.Application.Tests

## Purpose

- xUnit test suite validating the ASP.NET Core host (`LibraryManagement.Application`).
- Covers HTTP endpoint behaviour, module composition, and application service orchestration.

## Dependencies

- Depends on `LibraryManagement.Application` via project reference.
- Requires NuGet packages `xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`, and `coverlet.collector`.

## Directory Layout

```
LibraryManagement.Application.Tests/
  LibraryManagement.Application.Tests.csproj
  README.md
  UnitTest1.cs
```

## Commands

```bash
# Restore required packages
dotnet restore

# Run application-layer tests
dotnet test
```

## Tests

- Add integration tests for key endpoints (checkout, returns, holds) using the ASP.NET Core test host.
- Include contract tests for application ports and adapters as they are introduced.
- Use fixtures to share host setup when tests require consistent state.

## Integration Points

- Validates inbound HTTP adapters and their orchestration of domain logic.
- Mock outbound dependencies (persistence, messaging) until concrete adapters exist.

## Environment & Configuration

- Configure test-specific settings via `appsettings.json` overrides or in-memory configuration providers.
- Avoid reliance on external services unless explicitly required for integration coverage.

## Related Documentation

- `../../docs/architecture.md`
- `../../docs/ai-collaboration.md`
- `../../docs/project-roadmap.md`

## Maintenance Notes

- Replace `UnitTest1` with purposeful test classes covering real scenarios.
