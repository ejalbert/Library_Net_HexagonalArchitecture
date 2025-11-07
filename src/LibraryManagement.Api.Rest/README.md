# LibraryManagement.Api.Rest

## Purpose

- Provides the REST delivery adapter for the hexagonal architecture.
- Hosts HTTP controllers, DTO mappers, and OpenAPI metadata for catalogue, circulation, and admin scenarios.
- Encapsulates configuration needed to plug REST endpoints into any ASP.NET Core host via the module bootstrapper.

## Dependencies

- References `LibraryManagement.Api.Rest.Client` for DTOs and request/response contracts.
- References `LibraryManagement.Domain` for domain orchestration.
- References `LibraryManagement.ModuleBootstrapper.AspNetCore` to align with the module registration pipeline.
- Uses the `Microsoft.AspNetCore.OpenApi` package to expose Swagger endpoints.

## Directory Layout

```
LibraryManagement.Api.Rest/
  Domains/
    Books/
  ModuleConfigurations/
    ApiModule.cs
    RestApiModuleEnvConfiguration.cs
    RestApiModuleOptions.cs
  LibraryManagement.Api.Rest.csproj
  README.md
```

## Commands

```bash
# Restore and build the REST adapter
dotnet restore
dotnet build

# Execute the paired integration tests
dotnet test ../../tests/LibraryManagement.Api.Rest.Tests/LibraryManagement.Api.Rest.Tests.csproj
```

## Tests

- Validated by `LibraryManagement.Api.Rest.Tests` using NUnit.
- Add component/integration tests per controller to ensure correct routing, model binding, and status codes.
- Cover DTO mappers so contract drift between server and client packages is detected early.

## Integration Points

- Use `builder.InitializeApplicationModuleConfiguration().AddRestApiModule()` inside the host project to register REST services.
- Use `app.UseApplicationModules().UseRestApiModule()` to wire middleware, routing, and OpenAPI exposure.
- Exposes `IBookDtoMapper` and similar abstractions for other domains.

## Environment & Configuration

- Binds the `RestApi` configuration section to `RestApiModuleOptions` (currently `BasePath`).
- Default base path is `/api`. Override per environment via `appsettings.{Environment}.json` or environment variables (`RestApi__BasePath`).

## Related Documentation

- `../../docs/architecture.md`
- `../../docs/project-roadmap.md`
- `../../docs/ai-collaboration.md`

## Maintenance Notes

- Replace the placeholder book endpoints with real catalogue/circulation controllers as use cases mature.
- Keep OpenAPI descriptions in sync with the shared client package so consumers remain compatible.
