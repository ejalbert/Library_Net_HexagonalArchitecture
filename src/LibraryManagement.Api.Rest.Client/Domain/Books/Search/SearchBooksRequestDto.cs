using LibraryManagement.Api.Rest.Client.Common.Searches;

namespace LibraryManagement.Api.Rest.Client.Domain.Books.Search;

public record SearchBooksRequestDto(string? SearchTerm = null, PaginationDto? Pagination = null) : SearchRequestDtoBase(SearchTerm, Pagination);

public record SearchBooksResponseDto(IEnumerable<BookDto> Books);
