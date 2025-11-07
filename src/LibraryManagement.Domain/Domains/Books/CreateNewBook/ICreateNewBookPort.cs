namespace LibraryManagement.Domain.Domains.Books.CreateNewBook;

public interface ICreateNewBookPort
{
    Task<Book> Create(string title);
}