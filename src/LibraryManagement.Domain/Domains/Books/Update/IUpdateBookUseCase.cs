using LibraryManagement.Domain.Domains.Books;

namespace LibraryManagement.Domain.Domains.Books.Update;

public interface IUpdateBookUseCase
{
    Task<Book> Update(UpdateBookCommand command);
}
