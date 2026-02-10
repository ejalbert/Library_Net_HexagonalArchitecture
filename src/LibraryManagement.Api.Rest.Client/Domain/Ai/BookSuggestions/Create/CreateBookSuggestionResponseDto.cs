using System.ComponentModel;

namespace LibraryManagement.Api.Rest.Client.Domain.Ai.BookSuggestions.Create;

[Description("Response DTO for book suggestions")]
public record CreateBookSuggestionResponseDto(
    [property: Description("Generated book suggestion")]
    string Suggestion);
