using LibraryManagement.Api.Rest.Client.Common.Searches;

namespace LibraryManagement.Api.Rest.Client.Domain.Authors.Search;

public record SearchAuthorsRequestDto(string? SearchTerm = null, PaginationDto? Pagination = null)
    : SearchRequestDtoBase(SearchTerm, Pagination);

public record SearchAuthorsResponseDto(IEnumerable<AuthorDto> Results, PaginationInfoDto Pagination)
    : SearchResponseDtoBase<AuthorDto>(Results, Pagination);
