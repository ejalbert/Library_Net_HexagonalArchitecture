namespace LibraryManagement.Domain.Domains.Books.Delete;

public interface IDeleteBookUseCase
{
    Task Delete(DeleteBookCommand command);
}
