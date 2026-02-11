using System.ComponentModel;

namespace LibraryManagement.Api.Rest.Client.Domain.Ai.BookSuggestions.Create;

[Description("Request DTO for creating a book suggestion")]
public record CreateBookSuggestionRequestDto(
    [property: Description("Prompt used to generate a book suggestion")]
    string Prompt);
