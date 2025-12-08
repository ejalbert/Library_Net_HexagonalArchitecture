using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Books;

namespace LibraryManagement.AI.SemanticKernel.Domain.Books.Plugins;

public interface ISearchBooksPlugin
{
    Task<SearchResult<Book>> SearchBooksAsync(string searchTerm, Pagination pagination);
}
