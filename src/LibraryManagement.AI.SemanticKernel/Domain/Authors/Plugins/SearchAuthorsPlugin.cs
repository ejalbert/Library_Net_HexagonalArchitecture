using System.ComponentModel;

using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Domain.Domains.Authors.Search;

using Microsoft.SemanticKernel;

namespace LibraryManagement.AI.SemanticKernel.Domain.Authors.Plugins;

public class SearchAuthorsPlugin(ISearchAuthorsUseCase searchAuthorsUseCase)  : ISearchAuthorPlugin
{
    [KernelFunction("search_authors")]
    [Description("""
                 Search for authors in the library catalog.
                 """)]
    public Task<SearchResult<Author>> SearchAuthorsAsync(
        [Description("The term to search for authors. Only author name are supported currently. Providing an empty string will return all authors.")]
        string searchTerm,
        Pagination pagination)
    {
        return searchAuthorsUseCase.Search(new(searchTerm, pagination));
    }
}
