namespace LibraryManagement.Domain.Domains.Books.CreateNewBook;

public interface ICreateNewBookUseCase
{
    Task<Book> Create(CreateNewBookCommand command);
}