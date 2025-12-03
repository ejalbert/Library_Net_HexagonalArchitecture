using LibraryManagement.Api.Rest.Client.Domain.BookSuggestions.Create;
using LibraryManagement.Domain.Domains.BookSuggestions.Create;

using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Domains.BookSuggestions.Create;

public interface ICreateBookSuggestionController
{
    Task<IResult> CreateBookSuggestion(CreateBookSuggestionRequestDto request);
}

public class CreateBookSuggestionController(ICreateBookSuggestionUseCase createBookSuggestionUseCase) : ICreateBookSuggestionController
{
    public async Task<IResult> CreateBookSuggestion(CreateBookSuggestionRequestDto request)
    {
        var command = new CreateBookSuggestionCommand(request.Prompt);


        string suggestion = await createBookSuggestionUseCase.SuggestAsync(command);

        return Results.Ok(new CreateBookSuggestionResponseDto(suggestion));
    }
}
