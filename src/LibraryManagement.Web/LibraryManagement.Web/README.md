# LibraryManagement.Web

Blazor Server host that renders the Library Management UI and composes the Web client module.

## Responsibilities

- Bootstraps the Web module via the module bootstrapper extensions (`AddWebModule()` / `UseWebModule()`).
- Serves Razor components with both interactive server and WebAssembly render modes plus static assets.
- Wires the typed REST API client (via `RestApi:BasePath`) so components can query the backend.
- Registers UI-facing domain services such as the author model mapper.

## Project Layout

```
LibraryManagement.Web/
  Program.cs                         # Composition root calling AddWebModule/UseWebModule
  ModuleConfigurations/WebModule.cs  # Module registration + middleware mapping
  Components/                        # Razor components (layout, pages)
  Domain/                            # UI-facing models + mappers (e.g., authors)
  wwwroot/                           # Static assets and CSS
  appsettings*.json                  # Host configuration
```

## Commands

```bash
# Restore + build
dotnet restore
dotnet build src/LibraryManagement.Web/LibraryManagement.Web/LibraryManagement.Web.csproj

# Run the Blazor Server host
dotnet run --project src/LibraryManagement.Web/LibraryManagement.Web/LibraryManagement.Web.csproj
```

## Configuration

- `RestApi:BasePath` – forwarded to the REST client module; defaults to `http://localhost:5007/api/` when unset.
- Standard ASP.NET Core logging/hosting settings via `appsettings*.json`.

## Tests

- `tests/LibraryManagement.Web.Tests` exercises the Blazor components with bUnit (see README for the naming rule that pairs `.razor` tests with `.razor.cs`).
