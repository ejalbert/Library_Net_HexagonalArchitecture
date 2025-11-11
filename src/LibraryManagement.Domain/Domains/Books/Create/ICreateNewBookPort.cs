namespace LibraryManagement.Domain.Domains.Books.Create;

public interface ICreateNewBookPort
{
    Task<Book> Create(string title);
}
