# LibraryManagement.Api.Rest.Client

Shared DTOs and typed HTTP clients that mirror the REST delivery module. Consumers (UI, workers, tests) depend on this package to avoid duplicating contract knowledge.

## Key Capabilities

- Defines `BookDto`, `CreateNewBookRequestDto`, `UpdateBookRequestDto`, and `SearchBooksRequest/ResponseDto` types.
- Implements `IBooksClient` with `Create`, `Get`, `Update`, `Search`, and `Delete` operations that call `/api/v1/books` endpoints.
- Provides `IRestAPiClient` plus `AddRestApiHttpClient()` so hosts can register a configured `HttpClient` based on the `RestApi` configuration section.
- Includes a helper extension (`restAPiClient.Books`) to obtain the typed client from an injected `IRestAPiClient`.

## Dependencies

- `Microsoft.Extensions.Http` – used to register the typed `HttpClient` inside DI.
- `LibraryManagement.ModuleBootstrapper` – makes the configuration helper align with the rest of the module system.

## Directory Layout

```
LibraryManagement.Api.Rest.Client/
  Domain/Books/
    BookDto.cs
    BooksClient.cs
    IBooksClient.cs
    Create/
    Search/
  ModuleConfigurations/
    RestApiClientModule*.cs
  RestApiClient.cs
  IRestAPiClient.cs
```

## Commands

```bash
# Build the client library
dotnet build

# Execute the paired contract tests
dotnet test ../../tests/LibraryManagement.Api.Rest.Client.Tests/LibraryManagement.Api.Rest.Client.Tests.csproj
```

## Tests

`LibraryManagement.Api.Rest.Client.Tests` uses custom `HttpMessageHandler` doubles to assert that the typed client issues the correct HTTP verbs, URLs, and payloads. It also ensures failures (e.g., 500 responses) throw meaningful exceptions.

## Configuration

`AddRestApiHttpClient()` binds the `RestApi` configuration section and defaults the base URL to `http://localhost:5007/api` when nothing is supplied. Override it per environment (e.g., `RestApi__BasePath=https://catalogue.local/api`).

Keep this README updated as additional REST areas (circulation, patrons, etc.) are added.
