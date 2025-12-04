namespace LibraryManagement.Domain.Domains.Ai.BookSuggestions.Create;

public interface ICreateBookSuggestionUseCase
{
    Task<string> SuggestAsync(CreateBookSuggestionCommand command);
}
