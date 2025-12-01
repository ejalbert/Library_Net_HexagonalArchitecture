using LibraryManagement.Domain.Common.Searches;

namespace LibraryManagement.Domain.Domains.Books.Search;

public interface ISearchBooksUseCase
{
    Task<SearchResult<Book>> Search(SearchBooksCommand command);
}
