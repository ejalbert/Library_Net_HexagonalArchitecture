# LibraryManagement.Web.Tests

bUnit regression tests for the Blazor UI.

## Coverage

- `Components/BookPageTests.razor` verifies the book listing renders data from the REST client stub.
- `Components/AuthorPageTests.razor` exercises the author creation flow end-to-end in the component.
- Each test component has a matching partial `.razor.cs` file to satisfy the discovery rule noted in the root README.

## Running

```bash
dotnet test tests/LibraryManagement.Web.Tests/LibraryManagement.Web.Tests.csproj
```

Tests stub the typed REST client with a custom `HttpMessageHandler`, so no real backend services are required.
