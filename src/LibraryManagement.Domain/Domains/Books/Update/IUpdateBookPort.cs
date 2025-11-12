using LibraryManagement.Domain.Domains.Books;

namespace LibraryManagement.Domain.Domains.Books.Update;

public interface IUpdateBookPort
{
    Task<Book> Update(string id, string title, string authorId);
}
