namespace LibraryManagement.Domain.Domains.Books.Create;

public interface ICreateNewBookUseCase
{
    Task<Book> Create(CreateNewBookCommand command);
}
