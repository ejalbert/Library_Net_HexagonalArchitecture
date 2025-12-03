# LibraryManagement.AI.OpenAi

Infrastructure module that provides OpenAI-powered capabilities for the library domain. The module implements outbound ports for AI-driven features such as book suggestions and exposes chat tools for searching books and authors.

## Key Capabilities

- Registers OpenAI services through `AddOpenAiModule()` with configurable API key and model selection.
- Configures a singleton `ChatClient` from the OpenAI SDK for chat completion interactions.
- Implements `ICreateBookSuggestionPort` to generate AI-powered book recommendations.
- Provides chat tools for searching books and authors via AI function calling.
- Binds `OpenAiModuleOptions` from configuration section `OpenAi`.

## Directory Layout

```
LibraryManagement.AI.OpenAi/
  Domain/
    Authors/
      AuthorServiceRegistration.cs
      Tools/
        ISearchAuthorsChatTool.cs
        SearchAuthorsChatTools.cs
    Books/
      BookServiceRegistration.cs
      Tools/
        ISearchBooksChatTool.cs
        SearchBooksChatTool.cs
    BookSuggestion/
      Adapters/
        CreateBookSuggestionAdapter.cs
      BookSuggestionServiceRegistration.cs
    Common/
  ModuleConfigurations/
    OpenAiModule.cs
    OpenAiModuleEnvConfiguration.cs
    OpenAiModuleOptions.cs
```

## Configuration

The module reads configuration from the `OpenAi` section:

```json
{
  "OpenAi": {
    "ApiKey": "your-api-key-here",
    "Model": "gpt-4.1-nano"
  }
}
```

### Configuration Options

- **ApiKey** (required): Your OpenAI API key for authentication.
- **Model** (optional): The OpenAI model to use. Defaults to `gpt-4.1-nano` if not specified.

### Keeping API Keys Secure for Local Development

**Never commit API keys to version control.** Here are recommended approaches for local development:

#### Option 1: User Secrets (Recommended for .NET)

Use .NET's user secrets to store your API key locally without affecting source control:

```bash
# Navigate to the consuming application (e.g., LibraryManagement.Application)
cd src/LibraryManagement.Application

# Set the API key
dotnet user-secrets set "OPENAI:APIKEY" "your-api-key-here"

# Optionally set a different model
dotnet user-secrets set "OpenAi:Model" "gpt-4-turbo"
```

User secrets are stored in your user profile directory and are automatically loaded in development environments.

#### Option 2: Environment Variables

Set environment variables in your shell or IDE:

```bash
# In your terminal (macOS/Linux)
export OpenAi__ApiKey="your-api-key-here"
export OpenAi__Model="gpt-4.1-nano"

# Or add to your shell profile (~/.zshrc, ~/.bashrc)
echo 'export OpenAi__ApiKey="your-api-key-here"' >> ~/.zshrc
```

Note: Use double underscores (`__`) to represent nested configuration sections in environment variables.

#### Option 3: appsettings.Development.json (Local Only)

Create a local `appsettings.Development.json` file that is ignored by git (ensure `.gitignore` includes `appsettings.Development.json` with sensitive data):

```json
{
  "OpenAi": {
    "ApiKey": "your-api-key-here",
    "Model": "gpt-4.1-nano"
  }
}
```

**Important:** Ensure this file is in `.gitignore` if it contains real secrets. The current `.gitignore` includes `*.user` files but not development appsettings by default.

#### Option 4: .env Files

If using a `.env` file loader, create a `.env` file in the application root (already in `.gitignore`):

```env
OpenAi__ApiKey=your-api-key-here
OpenAi__Model=gpt-4.1-nano
```

## Usage

Register the module in your application's `Program.cs`:

```csharp
builder
    .InitializeApplicationModuleConfiguration()
    .AddDomainModule()
    .AddPersistencePostgresModule()
    .AddOpenAiModule() // Registers OpenAI services
    .AddRestApiModule();
```

Optionally override configuration via delegate:

```csharp
builder.AddOpenAiModule(options =>
{
    options.ApiKey = "override-key";
    options.Model = "gpt-4-turbo";
});
```

## Commands

```bash
# Build the OpenAI module
dotnet build

# Execute module tests (if tests exist)
dotnet test ../../tests/LibraryManagement.AI.OpenAi.Tests/LibraryManagement.AI.OpenAi.Tests.csproj
```

## Dependencies

- `OpenAI` SDK (via NuGet) for chat completion API.
- `LibraryManagement.Domain` for use-case interfaces and outbound ports.
- `LibraryManagement.ModuleBootstrapper` for module registration patterns.
- `Microsoft.Extensions.Options` for configuration binding.

## Integration Points

- **Outbound adapter**: Implements `ICreateBookSuggestionPort` to generate book suggestions using OpenAI.
- **Chat tools**: Provides `ISearchBooksChatTool` and `ISearchAuthorsChatTool` for AI function calling scenarios.
- **No direct persistence**: This module focuses on AI integration and delegates data operations to domain use cases.

## Testing Considerations

When testing components that depend on OpenAI:

- Mock `ChatClient` or the tool interfaces (`ISearchBooksChatTool`, etc.) to avoid real API calls.
- Use test API keys in CI/CD via secure environment variables or secrets management.
- Consider using recorded responses or test fixtures for predictable behavior.

## Future Enhancements

Document additional AI capabilities here as they are added (e.g., semantic search, content generation, RAG pipelines).

