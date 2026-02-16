namespace LibraryManagement.Domain.Domains.Ai.BookSuggestions.Create;

public class CreateBookSuggestionService(ICreateBookSuggestionPort createBookSuggestionPort)
    : ICreateBookSuggestionUseCase
{
    public Task<string> SuggestAsync(CreateBookSuggestionCommand command)
    {
        return createBookSuggestionPort.SuggestAsync(command.Prompt);
    }
}
