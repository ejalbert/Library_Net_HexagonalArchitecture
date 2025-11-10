namespace LibraryManagement.Domain.Domains.Books.Search;

public interface ISearchBooksPort
{
    Task<IEnumerable<Book>> Search(string? searchTerm);
}