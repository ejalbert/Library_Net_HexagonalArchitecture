# LibraryManagement.Api.Rest.Tests

xUnit suite that protects the REST delivery module (`LibraryManagement.Api.Rest`). Tests spin up minimal `WebApplicationBuilder` instances to verify option binding, service registration, and endpoint templates without hosting a full app.

## Current Coverage

- `ApiModuleTests` asserts configuration precedence (environment vs. delegate overrides).
- Verifies DI registrations for controllers/mappers are available once `AddRestApiModule()` runs.
- Ensures `UseRestApiModule()` maps `/api/v1/books`, `/api/v1/books/{id}`, and `/api/v1/books/search` routes.

## Commands

```bash
# Restore + execute tests
dotnet test
```

Add controller-level tests (model validation, response codes) as new endpoints are introduced.
