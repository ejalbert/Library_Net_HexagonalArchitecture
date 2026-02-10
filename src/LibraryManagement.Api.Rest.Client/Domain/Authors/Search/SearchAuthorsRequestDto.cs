using System.ComponentModel;
using LibraryManagement.Api.Rest.Client.Common.Searches;

namespace LibraryManagement.Api.Rest.Client.Domain.Authors.Search;

[Description("Request DTO for searching authors")]
public record SearchAuthorsRequestDto(
    [property: Description("Search term to filter authors")]
    string? SearchTerm = null,
    [property: Description("Pagination options for the search results")]
    PaginationDto? Pagination = null)
    : SearchRequestDtoBase(SearchTerm, Pagination);

[Description("Response DTO for searching authors")]
public record SearchAuthorsResponseDto(
    [property: Description("Author search results")]
    IEnumerable<AuthorDto> Results,
    [property: Description("Pagination info for the results")]
    PaginationInfoDto Pagination)
    : SearchResponseDtoBase<AuthorDto>(Results, Pagination);
