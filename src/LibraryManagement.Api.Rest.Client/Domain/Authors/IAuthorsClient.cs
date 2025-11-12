using LibraryManagement.Api.Rest.Client.Domain.Authors.Create;

namespace LibraryManagement.Api.Rest.Client.Domain.Authors;

public interface IAuthorsClient
{
    Task<AuthorDto> Create(CreateAuthorRequestDto requestDto, CancellationToken cancellationToken = default);
}
