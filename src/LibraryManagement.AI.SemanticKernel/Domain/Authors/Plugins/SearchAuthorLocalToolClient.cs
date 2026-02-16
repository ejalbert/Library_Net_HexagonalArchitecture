using LibraryManagement.AI.SemanticKernel.LocalTools.Tools.Authors;
using LibraryManagement.AI.SemanticKernel.ModuleConfigurations;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Search;
using LibraryManagement.Domain.Common.Searches;

namespace LibraryManagement.AI.SemanticKernel.Domain.Authors.Plugins;

public class SearchAuthorLocalToolClient(ILocalToolClient localToolClient) : ISearchAuthorLocalToolClient
{
    public Task<SearchAuthorsResponseDto> SearchAuthorsAsync(string searchTerm, Pagination pagination,
        CancellationToken cancellationToken = default)
    {
        return localToolClient.SendAsync<SearchAuthorsResponseDto>(SearchAuthorsTool.ToolName, searchTerm,
            pagination.PageIndex, pagination.PageSize, cancellationToken);
    }
}
