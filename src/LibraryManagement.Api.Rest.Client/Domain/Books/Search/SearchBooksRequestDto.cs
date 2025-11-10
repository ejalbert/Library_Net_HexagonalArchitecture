namespace LibraryManagement.Api.Rest.Client.Domain.Books.Search;

public record SearchBooksRequestDto(string? SearchTerm = null);

public record SearchBooksResponseDto(IEnumerable<BookDto> Books);