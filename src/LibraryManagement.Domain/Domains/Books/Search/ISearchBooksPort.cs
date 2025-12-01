using LibraryManagement.Domain.Common.Searches;

namespace LibraryManagement.Domain.Domains.Books.Search;

public interface ISearchBooksPort
{
    Task<SearchResult<Book>> Search(string? searchTerm, Pagination pagination);
}
