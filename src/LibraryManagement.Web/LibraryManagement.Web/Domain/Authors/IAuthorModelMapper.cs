using LibraryManagement.Api.Rest.Client.Generated.Model;

namespace LibraryManagement.Web.Domain.Authors;

public interface IAuthorModelMapper
{
    public AuthorModel ToModel(AuthorDto dto);

    public CreateAuthorRequestDto ToCreateAuthorRequestDto(AuthorModel model);
}
