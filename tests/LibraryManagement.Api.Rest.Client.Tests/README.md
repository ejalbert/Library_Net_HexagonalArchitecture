# LibraryManagement.Api.Rest.Client.Tests

Validates the typed REST client contracts using xUnit. Each test injects a custom `HttpMessageHandler` to assert
outbound HTTP requests and simulate server responses.

## Current Coverage

- `BooksClientTests` cover `Create`, `Get`, `Update`, `Search`, and `Delete` flows plus error handling (non-success
  responses throw `HttpRequestException`).
- `TestHttpMessageHandler` centralises request interception so scenarios remain deterministic.

## Commands

```bash
# Run client contract tests
dotnet test
```

Extend the suite whenever new DTOs or typed clients are added to the package.
