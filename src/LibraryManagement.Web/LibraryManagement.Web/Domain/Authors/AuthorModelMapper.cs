using LibraryManagement.Api.Rest.Client.Generated.Model;

using Riok.Mapperly.Abstractions;

namespace LibraryManagement.Web.Domain.Authors;

[Mapper]
public partial class AuthorModelMapper : IAuthorModelMapper
{
    public partial AuthorModel ToModel(AuthorDto dto);

    [MapperIgnoreSource(nameof(AuthorModel.Id))]
    public partial CreateAuthorRequestDto ToCreateAuthorRequestDto(AuthorModel model);
}
