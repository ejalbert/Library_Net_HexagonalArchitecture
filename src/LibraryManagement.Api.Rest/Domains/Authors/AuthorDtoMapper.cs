using LibraryManagement.Api.Rest.Client.Domain.Authors;
using LibraryManagement.Domain.Domains.Authors;

using Riok.Mapperly.Abstractions;

namespace LibraryManagement.Api.Rest.Domains.Authors;

[Mapper]
public partial class AuthorDtoMapper : IAuthorDtoMapper
{
    public partial AuthorDto ToDto(Author author);
}
