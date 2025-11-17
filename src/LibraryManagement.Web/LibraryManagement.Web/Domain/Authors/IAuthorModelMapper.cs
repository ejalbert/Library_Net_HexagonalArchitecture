using LibraryManagement.Api.Rest.Client.Domain.Authors;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Create;

namespace LibraryManagement.Web.Domain.Authors;

public interface IAuthorModelMapper
{
     public AuthorModel ToModel(AuthorDto dto);

     public CreateAuthorRequestDto ToCreateAuthorRequestDto(AuthorModel model);
}
