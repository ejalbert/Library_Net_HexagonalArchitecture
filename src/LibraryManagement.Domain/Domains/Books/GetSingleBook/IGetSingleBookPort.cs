namespace LibraryManagement.Domain.Domains.Books.GetSingleBook;

public interface IGetSingleBookPort
{
    public Task<Book> GetById(string id);
}