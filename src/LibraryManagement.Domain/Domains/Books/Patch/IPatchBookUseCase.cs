namespace LibraryManagement.Domain.Domains.Books.Patch;

public interface IPatchBookUseCase
{
    Task<Book> Patch(PatchBookCommand command);
}
