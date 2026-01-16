# LibraryManagement.Clients.Desktop

WPF desktop client that consumes the REST API to display books. Uses the module bootstrapper for DI setup and ships localized UI strings.

## Purpose

- Compose a desktop UI that lists books via the typed REST client.
- Demonstrate module bootstrapper usage (`AddDesktopModule()`) in a WPF host.
- Provide localization support for UI strings (e.g., book list labels).

## Dependencies

- `CommunityToolkit.Mvvm` for view models and commands.
- `Microsoft.Extensions.Hosting` / `Microsoft.Extensions.Localization` for DI and localization.
- `LibraryManagement.Api.Rest.Client` for typed REST calls (`IRestAPiClient`).

## Directory Layout

```
LibraryManagement.Clients.Desktop/
  App.xaml(.cs)                     # WPF host bootstrapping AddDesktopModule()
  MainWindow.xaml(.cs)              # Shell window hosting book list component
  ModuleConfigurations/DesktopModule.cs
  Domain/Books/                     # Book list component, view model, resources
  Localization/                     # Markup extension for localized strings
  Mvvm/                             # Shared MVVM interfaces
  Resources/                        # Global resources (e.g., Strings.resx)
  LibraryManagement.Clients.Desktop.csproj
```

## Commands

Run from this directory unless noted otherwise.

```bash
# Restore + build the WPF client
dotnet restore
dotnet build

# Run the desktop app (requires the REST API running and accessible)
dotnet run --project LibraryManagement.Clients.Desktop.csproj
```

## Tests

- No `{Project}.Tests` project yet. Add WPF/UI tests or view-model unit tests alongside this project to cover the book list component and REST client integration.

## Integration Points

- Registers services via `AddDesktopModule()`:
  - `AddRestApiHttpClient()` binds the `RestApi` section to create `IRestAPiClient`.
  - Book list component/view model (`BookList.xaml` / `BookListViewModel`) uses the typed client to search books.
- Uses localization resources (`BookStrings.resx`, `BookStrings.fr.resx`) via `LocExtension`.
- Resolves view models from `Ioc.Default` in XAML with `ResolveExtension` when needed.

## Environment & Configuration

- `RestApi:BasePath` must point to a reachable REST API (defaults to `http://localhost:5007/api` in the REST client module).
- Ensure the backend API is running before launching the desktop client.

## Related Documentation

- `../../docs/architecture.md` for module composition patterns.
- `../../docs/ai-collaboration.md` for collaboration/testing expectations.
