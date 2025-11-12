using LibraryManagement.Api.Rest.Client.Domain.Books.Create;
using LibraryManagement.Domain.Domains.Books.Create;

using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Domains.Books.CreateNewBook;

public class CreateNewBookController(ICreateNewBookUseCase createNewBookUseCase, IBookDtoMapper mapper) : ICreateNewBookController
{
    public async Task<IResult> CreateNewBook(CreateNewBookRequestDto requestDto)
    {
        var book = await createNewBookUseCase.Create(
            new CreateNewBookCommand(requestDto.Title, requestDto.AuthorId)
        );

        return Results.Ok(mapper.ToDto(book));
    }
}
