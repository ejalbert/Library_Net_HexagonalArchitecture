namespace LibraryManagement.Domain.Domains.Books.GetSingleBook;

public interface IGetSingleBookUseCase
{
    public Task<Book> Get(GetSingleBookCommand command);
}