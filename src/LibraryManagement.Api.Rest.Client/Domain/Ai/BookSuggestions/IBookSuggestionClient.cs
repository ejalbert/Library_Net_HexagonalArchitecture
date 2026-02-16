using LibraryManagement.Api.Rest.Client.Domain.Ai.BookSuggestions.Create;

namespace LibraryManagement.Api.Rest.Client.Domain.Ai.BookSuggestions;

public interface IBookSuggestionClient
{
    Task<CreateBookSuggestionResponseDto> GetBookSuggestion(string prompt,
        CancellationToken cancellationToken = default);
}
