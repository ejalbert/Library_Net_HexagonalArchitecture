using LibraryManagement.Domain.Common.Searches;

namespace LibraryManagement.Domain.Domains.Books.Search;

public class SearchBooksService(ISearchBooksPort searchBooksPort) : ISearchBooksUseCase
{
    public Task<IEnumerable<Book>> Search(SearchBooksCommand command)
    {
        return searchBooksPort.Search(command.SearchTerm, command.Pagination ?? new Pagination(0, 10));
    }
}
