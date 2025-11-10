using System.Net.Mime;
using LibraryManagement.Api.Rest.Client.Domain.Books.Create;
using LibraryManagement.Domain.Domains.Books.Create;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Rest.Domains.Books.CreateNewBook;

public class CreateNewBookController(ICreateNewBookUseCase createNewBookUseCase, IBookDtoMapper mapper) : ICreateNewBookController
{
    public async Task<IResult> CreateNewBook(CreateNewBookRequestDto requestDto)
    {
        var book =  await createNewBookUseCase.Create(
            new CreateNewBookCommand(requestDto.Title)
        );

        return Results.Ok(mapper.ToDto(book));
    }
}