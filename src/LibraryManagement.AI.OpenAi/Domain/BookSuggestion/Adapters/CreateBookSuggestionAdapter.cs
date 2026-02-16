using LibraryManagement.Domain.Domains.Ai.BookSuggestions.Create;

namespace LibraryManagement.AI.OpenAi.Domain.BookSuggestion.Adapters;

public class CreateBookSuggestionAdapter(IBookSuggestionAgent bookSuggestionAgent) : ICreateBookSuggestionPort
{
    public Task<string> SuggestAsync(string prompt)
    {
        return bookSuggestionAgent.SuggestBooksAsync(prompt);
    }
}
