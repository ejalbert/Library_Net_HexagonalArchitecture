namespace LibraryManagement.Domain.Domains.Books.Search;

public class SearchBooksService(ISearchBooksPort searchBooksPort) : ISearchBooksUseCase
{
    public Task<IEnumerable<Book>> Search(SearchBooksCommand command)
    {
        return searchBooksPort.Search(command.SearchTerm);
    }
}
