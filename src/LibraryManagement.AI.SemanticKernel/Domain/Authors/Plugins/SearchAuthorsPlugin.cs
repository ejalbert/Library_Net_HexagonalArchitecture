using System.ComponentModel;

using LibraryManagement.Api.Rest.Client.Domain.Authors.Search;
using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Domain.Domains.Authors.Search;

using Microsoft.SemanticKernel;

namespace LibraryManagement.AI.SemanticKernel.Domain.Authors.Plugins;

public class SearchAuthorsPlugin(
    ISearchAuthorsUseCase searchAuthorsUseCase,
    ISearchAuthorLocalToolClient searchAuthorLocalToolClient) : ISearchAuthorPlugin
{
    [KernelFunction("search_authors")]
    [Description("""
                 Search for authors in the library catalog.
                 """)]
    public async Task<SearchResult<Author>> SearchAuthorsAsync(
        [Description(
            "The term to search for authors. Only author name are supported currently. Providing an empty string will return all authors.")]
        string searchTerm,
        Pagination pagination)
    {
        SearchAuthorsResponseDto result = await searchAuthorLocalToolClient.SearchAuthorsAsync(searchTerm, pagination);

        return new SearchResult<Author>
        {
            Results = result.Results.Select(a => new Author
            {
                Id = a.Id,
                Name = a.Name
            }).ToList(),
            Pagination = new PaginationInfo
            {
                PageIndex = result.Pagination.PageIndex,
                PageSize = result.Pagination.PageSize,
                TotalItems = result.Pagination.TotalItems
            }
        };

        //return searchAuthorsUseCase.Search(new(searchTerm, pagination));
    }
}
