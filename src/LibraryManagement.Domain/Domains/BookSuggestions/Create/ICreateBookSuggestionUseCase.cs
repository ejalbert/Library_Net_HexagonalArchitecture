namespace LibraryManagement.Domain.Domains.BookSuggestions.Create;

public interface ICreateBookSuggestionUseCase
{
    Task<string> SuggestAsync(CreateBookSuggestionCommand command);
}
