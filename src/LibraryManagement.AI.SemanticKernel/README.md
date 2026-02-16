# LibraryManagement.AI.SemanticKernel

Semantic Kernel–powered AI adapter that wires book suggestion prompts and function-calling plugins (books/authors
search) into the domain. Registers chat completion services and bridges the `ICreateBookSuggestionPort` to an SK agent.

## Purpose

- Provide a Semantic Kernel implementation for AI-driven book suggestions using OpenAI chat completions.
- Expose SK plugins for searching books and authors via existing domain use cases so agents can ground responses in
  catalog data.
- Register chat completion services (token-aware wrapper + OpenAI connector) through the module bootstrapper.

## Dependencies

- `Microsoft.SemanticKernel`, `Microsoft.SemanticKernel.Connectors.OpenAI` for chat completions and plugins.
- `LibraryManagement.Domain` for search use cases and the `ICreateBookSuggestionPort` contract.
- Uses the shared module bootstrapper to plug into hosts (`AddSemanticKernelModule()`).

## Directory Layout

```
LibraryManagement.AI.SemanticKernel/
  Domain/
    Authors/Plugins/           # search_authors SK plugin calling ISearchAuthorsUseCase
    Books/Plugins/             # search_books SK plugin calling ISearchBooksUseCase
    BookSuggestions/           # BookSuggestionAgent + adapter for ICreateBookSuggestionPort
  ModuleConfigurations/        # Module wiring, env binding, options (ApiKey/Model)
  SemanticKernel/              # Token-aware chat completion service
  LibraryManagement.AI.SemanticKernel.csproj
```

## Commands

Run from this directory unless noted otherwise.

```bash
# Build the Semantic Kernel adapter
dotnet build
```

## Tests

- No dedicated test project yet; add `{Project}.Tests` alongside this project to cover BookSuggestionAgent behaviour,
  plugin wiring, and option binding.

## Integration Points

- Implements `ICreateBookSuggestionPort` via `CreateBookSuggestionAdapter` backed by `BookSuggestionAgent`.
- Registers SK plugins: `search_books` (uses `ISearchBooksUseCase`) and `search_authors` (uses `ISearchAuthorsUseCase`).
- Hosts obtain services through `AddSemanticKernelModule()`; also registers `IChatCompletionService` using the
  token-aware wrapper.

## Environment & Configuration

- Reads `OpenAi` configuration section:
    - `OpenAi:ApiKey` — required for chat completions (use user-secrets or env vars, never commit).
    - `OpenAi:Model` — model id, defaults to `gpt-4.1-nano` when unset.
- Options are bound inside `SemanticKernelModule`; you can override via the `configureOptions` delegate on
  `AddSemanticKernelModule(...)`.

## Related Documentation

- `../../docs/architecture.md` for adapter placement within the hexagonal layout.
- `../../docs/ai-collaboration.md` for collaboration and testing expectations.

## Maintenance Notes

- Add integration/unit tests once the adapter stabilises.
- Keep REST contracts and domain ports aligned if the plugin surface changes (e.g., additional AI tools).
