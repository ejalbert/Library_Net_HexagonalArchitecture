using System.ComponentModel;

using LibraryManagement.Api.Rest.Client.Common.Searches;

namespace LibraryManagement.Api.Rest.Client.Domain.Books.Search;

[Description("Request DTO for searching books")]
public record SearchBooksRequestDto(
    [property: Description("Search term to filter books")]
    string? SearchTerm = null,
    [property: Description("Pagination options for the search results")]
    PaginationDto? Pagination = null)
    : SearchRequestDtoBase(SearchTerm, Pagination);

[Description("Response DTO for searching books")]
public record SearchBooksResponseDto(
    [property: Description("Book search results")]
    IEnumerable<BookDto> Results,
    [property: Description("Pagination info for the results")]
    PaginationInfoDto Pagination)
    : SearchResponseDtoBase<BookDto>(Results, Pagination);
