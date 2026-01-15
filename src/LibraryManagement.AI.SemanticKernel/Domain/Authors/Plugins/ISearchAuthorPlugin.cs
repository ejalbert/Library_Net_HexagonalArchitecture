using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Authors;

namespace LibraryManagement.AI.SemanticKernel.Domain.Authors.Plugins;

public interface ISearchAuthorPlugin
{
    Task<SearchResult<Author>> SearchAuthorsAsync(string searchTerm, Pagination pagination);
}
