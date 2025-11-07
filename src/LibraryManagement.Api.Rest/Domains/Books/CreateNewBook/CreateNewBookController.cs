using System.Net.Mime;
using LibraryManagement.Domain.Domains.Books.CreateNewBook;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Rest.Domains.Books.CreateNewBook;

public class CreateNewBookController(ICreateNewBookUseCase createNewBookUseCase) : ICreateNewBookController
{
    public async Task<IResult> CreateNewBook(CreateNewBookRequestDto requestDto)
    {
        var book =  await createNewBookUseCase.Create(
            new CreateNewBookCommand(requestDto.Title)
        );

        return Results.Ok(book);
    }
}