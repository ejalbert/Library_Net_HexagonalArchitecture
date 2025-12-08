using System.ComponentModel;

using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Authors;

using Microsoft.SemanticKernel;

namespace LibraryManagement.AI.SemanticKernel.Domain.Authors.Plugins;

public interface ISearchAuthorPlugin
{
    Task<SearchResult<Author>> SearchAuthorsAsync(string searchTerm, Pagination pagination);
}
