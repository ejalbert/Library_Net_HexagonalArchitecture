using LibraryManagement.Api.Rest.Client.Domain.Authors;
using LibraryManagement.Domain.Domains.Authors;

namespace LibraryManagement.Api.Rest.Domains.Authors;

public interface IAuthorDtoMapper
{
    AuthorDto ToDto(Author author);
}
