namespace LibraryManagement.Domain.Domains.Books.GetSingle;

public interface IGetSingleBookPort
{
    public Task<Book> GetById(string id);
}
