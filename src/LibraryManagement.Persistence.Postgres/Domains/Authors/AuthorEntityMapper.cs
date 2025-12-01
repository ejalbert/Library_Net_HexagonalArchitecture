using LibraryManagement.Domain.Domains.Authors;

using Riok.Mapperly.Abstractions;

namespace LibraryManagement.Persistence.Postgres.Domains.Authors;

public interface IAuthorEntityMapper
{
    Author ToDomain(AuthorEntity authorEntity);
    AuthorEntity ToEntity(Author author);
}


[Mapper]
public partial class AuthorEntityMapper : IAuthorEntityMapper
{
    public partial AuthorEntity ToEntity(Author author);

    public partial Author ToDomain(AuthorEntity authorEntity);
}
