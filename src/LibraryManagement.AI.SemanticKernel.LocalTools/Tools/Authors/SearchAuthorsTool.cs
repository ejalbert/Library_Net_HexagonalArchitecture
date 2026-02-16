using LibraryManagement.Api.Rest.Client;
using LibraryManagement.Api.Rest.Client.Common.Searches;
using LibraryManagement.Api.Rest.Client.Domain.Authors;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Search;

using Microsoft.AspNetCore.SignalR.Client;

namespace LibraryManagement.AI.SemanticKernel.LocalTools.Tools.Authors;

public interface ISearchAuthorsTool : ILocalTool
{
}

public class SearchAuthorsTool(HubConnection connection, IRestAPiClient client)
    : LocalToolBase(connection), ISearchAuthorsTool
{
    public const string ToolName = "search_authors";

    public override Task RegisterAsync(CancellationToken cancellationToken = default)
    {
        Connection.On<string, string, int, int>(ToolName, async (correlationId, searchTerm, pageIndex, pageSize) =>
        {
            SearchAuthorsResponseDto result =
                await SearchAuthorsAsync(searchTerm, pageIndex, pageSize, cancellationToken);

            await SendToolResponseAsync(correlationId, ToolName, result);
        });
        return Task.CompletedTask;
    }

    public override Task UnregisterAsync(CancellationToken cancellationToken = default)
    {
        Connection.Remove(ToolName);

        return Task.CompletedTask;
    }

    private Task<SearchAuthorsResponseDto> SearchAuthorsAsync(string searchTerm, int pageIndex, int pageSize,
        CancellationToken cancellationToken)
    {
        return client.Authors.Search(new SearchAuthorsRequestDto(searchTerm, new PaginationDto(pageIndex, pageSize)),
            cancellationToken);
    }
}
