# LibraryManagement.Web.Client

Blazor WebAssembly client that ships alongside the server host to enable hybrid rendering.

## Responsibilities

- Configures the typed REST API client via `AddWebClientModule()` so components can call backend endpoints.
- Provides the WebAssembly entry point consumed by the server host through `AddAdditionalAssemblies`.
- Packages static assets and client-specific configuration under `wwwroot/`.

## Project Layout

```
LibraryManagement.Web.Client/
  Program.cs                              # WebAssembly bootstrapper
  ModuleConfigurations/WebClientModule.cs # Registers REST API HttpClient
  wwwroot/appsettings*.json               # Client-side config and logging
  Dockerfile                              # Builds the client bundle image
```

## Commands

```bash
# Restore + build
dotnet restore
dotnet build src/LibraryManagement.Web/LibraryManagement.Web.Client/LibraryManagement.Web.Client.csproj

# Publish the WebAssembly assets (placed under bin/.../publish/wwwroot)
dotnet publish src/LibraryManagement.Web/LibraryManagement.Web.Client/LibraryManagement.Web.Client.csproj -c Release
```

## Configuration

- `RestApi:BasePath` – base address for the typed REST client; defaults to `http://localhost:5007/api/`.
- Logging levels follow the usual `Logging:LogLevel` structure.
