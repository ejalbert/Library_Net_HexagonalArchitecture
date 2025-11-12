namespace LibraryManagement.Domain.Domains.Authors.Create;

public interface ICreateAuthorUseCase
{
    Task<Author> Create(CreateAuthorCommand command);
}
