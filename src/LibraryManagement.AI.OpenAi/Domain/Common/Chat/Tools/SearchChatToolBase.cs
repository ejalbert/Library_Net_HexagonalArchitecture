using System.Text.Json;

using LibraryManagement.Domain.Common.Searches;

using OpenAI.Chat;

namespace LibraryManagement.AI.OpenAi.Domain.Common.Chat.Tools;

public abstract class SearchChatToolBase<TResult>(string functionName, ChatTool tool) : IDomainChatTool
{
    public string FunctionName => functionName;
    public ChatTool Tool => tool;

    public async Task<string> InvokeAsync(ChatToolCall toolCall)
    {
        using var arguments = JsonDocument.Parse(toolCall.FunctionArguments);

        var hasSearchTerm =
            arguments.RootElement.TryGetProperty(nameof(SearchCommandBase.SearchTerm),
                out JsonElement searchTermProperty);

        var hasPagination =
            arguments.RootElement.TryGetProperty(nameof(SearchCommandBase.Pagination),
                out JsonElement paginationProperty);

        var searchTerm = hasSearchTerm ? searchTermProperty.GetString() : null;
        Pagination? pagination = hasPagination
            ? new Pagination(
                paginationProperty.GetProperty(nameof(Pagination.PageIndex)).GetInt32(),
                paginationProperty.GetProperty(nameof(Pagination.PageSize)).GetInt32())
            : null;

        SearchResult<TResult> result = await ExecuteSearchAsync(searchTerm, pagination, arguments);

        return JsonSerializer.Serialize(result);
    }

    protected abstract Task<SearchResult<TResult>> ExecuteSearchAsync(string? searchTerm, Pagination? pagination,
        JsonDocument arguments);
}
