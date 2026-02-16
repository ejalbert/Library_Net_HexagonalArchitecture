namespace LibraryManagement.AI.SemanticKernel.Domain.BookSuggestions;

public interface IBookSuggestionAgent
{
    Task<string> SuggestAsync(string prompt, CancellationToken cancellationToken = default);
}
