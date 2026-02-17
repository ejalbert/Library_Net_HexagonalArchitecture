<h1 style="text-align: center;">LibraryManagement REST API Client Generator</h1>

### About This Project

This project generates API clients for the LibraryManagement REST API from OpenAPI specifications. It produces:

- **TypeScript SDK** (npm package) using TypeScript-Axios
- **C# SDK** (NuGet package) using HttpClient

### Technical Stack

- **OpenAPI Generator**: Generates type-safe API clients from OpenAPI specs
- **TypeScript-Axios**: For TypeScript/JavaScript consumers
- **C# HttpClient**: For .NET consumers
- **Custom Mustache Templates**: Located in `openapi-generator-templates/` for customizing generated code

---

### How to Generate the SDKs

#### Prerequisites

1. **Node.js 20+** and npm installed
2. **.NET SDK 8.0+** installed (for C# client)
3. **Java Runtime** (required by OpenAPI Generator CLI)

#### Steps

1. **Start the LibraryManagement API**: Ensure the API is running locally (default: `http://localhost:5007`).

2. **Fetch the OpenAPI Spec**:
   ```bash
   npm run retreive-openapi-spec
   ```
   You can set `OPENAPI_URL` environment variable to override the default URL.

3. **Generate Both Clients**:
   ```bash
   npm run codegen
   ```
   Or generate individually:
   ```bash
   npm run codegen:ts      # TypeScript only
   npm run codegen:csharp  # C# only
   ```

4. **Build**:
   ```bash
   npm run build           # TypeScript SDK
   npm run build:csharp    # C# SDK
   npm run build:all       # Both
   ```

5. **Package for NuGet**:
   ```bash
   npm run pack:csharp     # Creates .nupkg in ./nupkg/
   ```

---

### Output Locations

| Client | Source | Build Output |
|--------|--------|--------------|
| TypeScript | `src/` | `dist/` |
| C# | `csharp-client/src/` | `csharp-client/src/.../bin/Release/` |
| NuGet Package | - | `nupkg/` |

---

### Configuring API Endpoints in .NET Minimal API

The generated clients use **tags** for class names and **operation names** for method names. Configure these in your Minimal API:

```csharp
RouteGroupBuilder group = app.MapGroup("/api/v1/books").WithTags("Books");

group.MapGet("{id}", handler)
    .WithName("GetBookById")           // → becomes method name
    .WithDescription("Get a book");    // → becomes method description
```

---

### C# Client Configuration

Edit `csharp-generator-config.json` to customize:

- `packageName`: NuGet package ID
- `targetFramework`: Target .NET version (default: `net8.0`)
- `nullableReferenceTypes`: Enable nullable reference types

### Version Synchronization

Both the TypeScript (npm) and C# (NuGet) client versions are automatically synchronized with the solution version via `Directory.Build.props` at the solution root.

**To update the version:**

1. Edit `Directory.Build.props` in the solution root:
   ```xml
   <LibraryManagementVersion>1.0.0</LibraryManagementVersion>
   ```

2. Regenerate the clients:
   ```bash
   npm run codegen        # Both TypeScript and C#
   npm run codegen:ts     # TypeScript only
   npm run codegen:csharp # C# only
   ```

The generation scripts read the version from `Directory.Build.props` and:
- **TypeScript**: Updates `package.json` version before generating
- **C#**: Injects `packageVersion` into the OpenAPI generator config

This ensures all projects (REST API, generated clients) share the same version.

---

### AppHost Integration

When running through the Aspire AppHost, use the **"Generate API Client"** command on the React client resource. This command:

1. Fetches the OpenAPI spec from the running API
2. Generates both TypeScript and C# clients
3. Builds and packs both packages
4. Copies packages to `local-libraries/` at the repo root:
   - `library-management-api-rest-client-generator-X.Y.Z.tgz` (TypeScript)
   - `LibraryManagement.Api.Rest.Client.Generated.X.Y.Z.nupkg` (C#)
5. Installs the TypeScript package in the React client

The `local-libraries/` folder serves as a local package repository for both npm and NuGet packages.

---

### Built With

- [OpenAPI Generator](https://openapi-generator.tech/)
- [Axios](https://github.com/axios/axios) (TypeScript)
- [HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient) (C#)
- [TypeScript](https://www.typescriptlang.org/)
- [Rollup](https://rollupjs.org/)
