using LibraryManagement.Api.Rest.Client.Domain.Authors.Search;
using LibraryManagement.Domain.Common.Searches;

namespace LibraryManagement.AI.SemanticKernel.Domain.Authors.Plugins;

public interface ISearchAuthorLocalToolClient
{
    Task<SearchAuthorsResponseDto> SearchAuthorsAsync(string searchTerm, Pagination pagination, CancellationToken cancellationToken = default);
}