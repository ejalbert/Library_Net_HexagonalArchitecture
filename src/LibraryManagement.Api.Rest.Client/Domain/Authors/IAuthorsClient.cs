using LibraryManagement.Api.Rest.Client.Domain.Authors.Create;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Search;

namespace LibraryManagement.Api.Rest.Client.Domain.Authors;

public interface IAuthorsClient
{
    Task<AuthorDto> Create(CreateAuthorRequestDto requestDto, CancellationToken cancellationToken = default);

    Task<SearchAuthorsResponseDto> Search(SearchAuthorsRequestDto requestDto,
        CancellationToken cancellationToken = default);
}
