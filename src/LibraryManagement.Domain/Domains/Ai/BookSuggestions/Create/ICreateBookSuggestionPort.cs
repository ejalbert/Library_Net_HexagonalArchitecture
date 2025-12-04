namespace LibraryManagement.Domain.Domains.Ai.BookSuggestions.Create;

public interface ICreateBookSuggestionPort
{
    Task<string> SuggestAsync(string prompt);
}
