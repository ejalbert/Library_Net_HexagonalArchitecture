namespace LibraryManagement.Domain.Domains.Authors.Create;

public interface ICreateAuthorPort
{
    Task<Author> Create(string name);
}
