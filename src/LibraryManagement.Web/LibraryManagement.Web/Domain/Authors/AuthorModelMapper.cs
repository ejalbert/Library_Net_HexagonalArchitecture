using LibraryManagement.Api.Rest.Client.Domain.Authors;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Create;

using Riok.Mapperly.Abstractions;

namespace LibraryManagement.Web.Domain.Authors;

[Mapper]
public partial class AuthorModelMapper : IAuthorModelMapper
{
    public partial AuthorModel ToModel(AuthorDto dto);

    [MapperIgnoreSource(nameof(AuthorModel.Id))]
    public partial CreateAuthorRequestDto ToCreateAuthorRequestDto(AuthorModel model);
}
