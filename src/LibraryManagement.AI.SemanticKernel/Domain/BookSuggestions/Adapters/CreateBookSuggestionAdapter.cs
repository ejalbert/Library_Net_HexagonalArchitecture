using LibraryManagement.Domain.Domains.Ai.BookSuggestions.Create;

namespace LibraryManagement.AI.SemanticKernel.Domain.BookSuggestions.Adapters;

public class CreateBookSuggestionAdapter(IBookSuggestionAgent bookSuggestionAgent): ICreateBookSuggestionPort
{
    public Task<string> SuggestAsync(string prompt)
    {
        return bookSuggestionAgent.SuggestAsync(prompt);
    }
}
