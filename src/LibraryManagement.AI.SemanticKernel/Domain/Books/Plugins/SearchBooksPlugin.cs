using System.ComponentModel;

using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Search;

using Microsoft.SemanticKernel;

namespace LibraryManagement.AI.SemanticKernel.Domain.Books.Plugins;

public class SearchBooksPlugin(ISearchBooksUseCase searchBooksUseCase) : ISearchBooksPlugin
{
    [KernelFunction("search_books")]
    [Description("""
                 Search for books in the library catalog. It is not possible to search by author name here.
                 You need to cross reference with the author id. For now the best approach is to search all books by using an empty search

                 the content returned will include alist of reesults with the books found, along with pagination information.
                 """)]
    public Task<SearchResult<Book>> SearchBooksAsync(
        [Description("The term to search for books. Typically a name or part of a name. can be empty to get all books")]
        string searchTerm, Pagination pagination)
    {
        return searchBooksUseCase.Search(new SearchBooksCommand(searchTerm, pagination));
    }
}
