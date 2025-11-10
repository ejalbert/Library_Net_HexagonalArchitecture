namespace LibraryManagement.Domain.Domains.Books.GetSingle;

public interface IGetSingleBookUseCase
{
    public Task<Book> Get(GetSingleBookCommand command);
}