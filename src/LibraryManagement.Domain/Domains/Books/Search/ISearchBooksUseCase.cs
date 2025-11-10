namespace LibraryManagement.Domain.Domains.Books.Search;

public interface ISearchBooksUseCase
{
    Task<IEnumerable<Book>> Search(SearchBooksCommand command);
}