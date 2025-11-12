namespace LibraryManagement.Domain.Domains.Books.Delete;

public interface IDeleteBookPort
{
    Task Delete(string id);
}
