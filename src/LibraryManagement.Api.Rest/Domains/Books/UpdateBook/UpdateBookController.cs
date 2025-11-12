using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Api.Rest.Client.Domain.Books.Update;
using LibraryManagement.Domain.Domains.Books.Update;

using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Domains.Books.UpdateBook;

public class UpdateBookController(IUpdateBookUseCase updateBookUseCase, IBookDtoMapper mapper) : IUpdateBookController
{
    public async Task<IResult> UpdateBook(string id, UpdateBookRequestDto requestDto)
    {
        var book = await updateBookUseCase.Update(new UpdateBookCommand(id, requestDto.Title, requestDto.AuthorId));

        return Results.Ok(mapper.ToDto(book));
    }
}
