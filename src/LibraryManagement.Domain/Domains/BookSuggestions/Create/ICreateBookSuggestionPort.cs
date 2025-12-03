namespace LibraryManagement.Domain.Domains.BookSuggestions.Create;

public interface ICreateBookSuggestionPort
{
    Task<string> SuggestAsync(string prompt);
}
