namespace LibraryManagement.AI.OpenAi.Domain.BookSuggestion;

public interface IBookSuggestionAgent
{
    Task<string> SuggestBooksAsync(string userPrompt, CancellationToken cancellationToken = default);
}
