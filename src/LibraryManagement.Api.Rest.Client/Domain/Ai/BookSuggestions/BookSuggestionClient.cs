using System.Net.Http.Json;

using LibraryManagement.Api.Rest.Client.Domain.Ai.BookSuggestions.Create;

namespace LibraryManagement.Api.Rest.Client.Domain.Ai.BookSuggestions;

public class BookSuggestionClient(RestApiClient client): IBookSuggestionClient
{
    public async Task<CreateBookSuggestionResponseDto> GetBookSuggestion(string prompt, CancellationToken cancellationToken)
    {
        var response = await client.HttpClient.PostAsJsonAsync("/api/v1/ai/book-suggestions",
            new CreateBookSuggestionRequestDto(prompt), cancellationToken);

        return (await response.Content.ReadFromJsonAsync<CreateBookSuggestionResponseDto>(cancellationToken))!;
    }
}
