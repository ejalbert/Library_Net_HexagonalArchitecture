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
    [MapperIgnoreTarget(nameof(AuthorEntity.Books))]
    [MapperIgnoreTarget(nameof(AuthorEntity.TenantId))]
    public partial AuthorEntity ToEntity(Author author);

    [MapperIgnoreSource(nameof(AuthorEntity.Books))]
    [MapperIgnoreSource(nameof(AuthorEntity.TenantId))]
    public partial Author ToDomain(AuthorEntity authorEntity);
}
