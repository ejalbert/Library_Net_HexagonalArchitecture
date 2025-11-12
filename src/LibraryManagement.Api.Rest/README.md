# LibraryManagement.Api.Rest

Delivery module that exposes domain capabilities via minimal APIs. The current implementation publishes `/api/v1/books` endpoints that forward to the book use cases.

## Key Capabilities

- Registers REST services through `AddRestApiModule()` and wires endpoints via `UseRestApiModule()`.
- Binds `RestApiModuleOptions` from configuration, defaulting to `/api` when not provided.
- Adds OpenAPI/Swagger in Development to document endpoints.
- Maps book endpoints for create, get-by-id, search, and delete scenarios using DTOs from the REST client package and Mapperly-based mappers.

## Directory Layout

```
LibraryManagement.Api.Rest/
  Domains/Books/
    BookServices.cs
    BookDtoMapper.cs
    CreateNewBook/*.cs
    DeleteBook/*.cs
    GetSingleBook/*.cs
    Search/*.cs
  ModuleConfigurations/
    ApiModule.cs
    RestApiModuleEnvConfiguration.cs
    RestApiModuleOptions.cs
```

## Commands

```bash
# Build the REST module
dotnet build

# Execute module tests
dotnet test ../../tests/LibraryManagement.Api.Rest.Tests/LibraryManagement.Api.Rest.Tests.csproj
```

## Dependencies

- `LibraryManagement.Api.Rest.Client` for shared DTOs and request models.
- `LibraryManagement.Domain` for the use-case interfaces.
- `LibraryManagement.ModuleBootstrapper.AspNetCore` so the module plugs into any ASP.NET Core host.
- `Microsoft.AspNetCore.OpenApi` for OpenAPI metadata.

## Configuration

Set `RestApi:BasePath` in host configuration to control the route prefix. When omitted, the module uses `/api` and then appends its group (`/v1/books`).

## Tests

`LibraryManagement.Api.Rest.Tests` verifies:

- Option binding precedence (configuration vs. delegate overrides).
- Service registration (controllers, mappers, DTO mapping).
- Endpoint templates via a minimal WebApplication builder.

Extend coverage with request/response validation as more endpoints are added.

## Integration Points

- Inbound adapters: minimal APIs defined in `BookServices.cs`.
- Outbound: depends on domain use cases (`ICreateNewBookUseCase`, etc.); there is no direct persistence coupling.

Document additional endpoint groups here whenever new domains are exposed.
