# LibraryManagement.Api.Rest.Client.Generator

## Purpose

- Generates API clients for the LibraryManagement REST API from OpenAPI specifications.
- Produces a TypeScript SDK (npm) and a C# SDK (NuGet).
- Acts as a tooling adapter for client distribution, not a runtime application module.

## Dependencies

- Node.js 20+ and npm for scripts and TypeScript build.
- Java runtime for OpenAPI Generator CLI.
- .NET SDK 8.0+ for the C# client build.
- Shared architecture docs: `../../docs/architecture.md`.

## Directory Layout

```
src/LibraryManagement.Api.Rest.Client.Generator/
├── csharp-client/                 # Generated C# client project
├── dist/                          # Built TypeScript SDK output
├── nupkg/                         # Packed NuGet artifacts
├── openapi-generator-templates/   # Mustache templates for OpenAPI Generator
├── openapi-specs/                 # Retrieved OpenAPI specs
├── scripts/                       # Generation/build scripts
├── src/                           # Generated TypeScript client source
├── csharp-generator-config.json   # C# generator configuration
├── openapitools.json              # OpenAPI Generator config
└── package.json                   # Node scripts and dependencies
```

## Commands

Run all commands from this directory unless noted otherwise.

```bash
# Fetch OpenAPI spec (API must be running)
npm run retreive-openapi-spec

# Generate both clients
npm run codegen

# Generate individually
npm run codegen:ts
npm run codegen:csharp

# Build outputs
npm run build
npm run build:csharp
npm run build:all

# Package C# client (creates .nupkg in ./nupkg/)
npm run pack:csharp
```

## Tests

- No dedicated test project for the generator yet.
- Validation is performed by running generation/build and consuming the produced packages.

## Integration Points

- Consumes OpenAPI from the running REST API (default `http://localhost:5007`).
- Emits npm and NuGet packages for downstream clients.
- AppHost can invoke generation via the "Generate API Client" command.
- Generated C# wrapper exposes `AddRestApiHttpClient` for DI registration.

## Environment & Configuration

- `OPENAPI_URL` overrides the default OpenAPI endpoint used by `npm run retreive-openapi-spec`.
- `csharp-generator-config.json` controls NuGet package settings:
  - `packageName`
  - `targetFramework` (default `net8.0`)
  - `nullableReferenceTypes`
- Versioning is synchronized with the solution root `Directory.Build.props`:
  - `LibraryManagementVersion` drives both TypeScript and C# package versions.

## Version Synchronization

1. Edit the solution root `Directory.Build.props`:
   ```xml
   <LibraryManagementVersion>1.0.0</LibraryManagementVersion>
   ```
2. Regenerate the clients:
   ```bash
   npm run codegen
   npm run codegen:ts
   npm run codegen:csharp
   ```

The generation scripts read the version and:
- Update the TypeScript `package.json` version.
- Inject `packageVersion` into the OpenAPI generator config for C#.

## AppHost Integration

When running through the Aspire AppHost, the "Generate API Client" command:
- Fetches the OpenAPI spec from the running API.
- Generates both TypeScript and C# clients.
- Builds and packs both packages.
- Copies packages to `local-libraries/` at the repo root.
- Installs the TypeScript package in the React client.

The `local-libraries/` folder serves as a local package repository for npm and NuGet packages.

## Related Documentation

- `../../docs/architecture.md`
- `../../docs/ai-collaboration.md`
- `../../src/LibraryManagement.Api.Rest/README.md`
- `../../src/LibraryManagement.Api.Rest.Client/README.md`

## Maintenance Notes

- Ensure REST API DTO changes are mirrored in the REST client contracts before regenerating.

## Built With

- OpenAPI Generator
- Axios (TypeScript)
- HttpClient (C#)
- TypeScript
- Rollup
