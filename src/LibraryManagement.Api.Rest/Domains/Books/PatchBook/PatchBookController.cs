using LibraryManagement.Api.Rest.Client.Domain.Books.Patch;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Patch;

using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Domains.Books.PatchBook;

public class PatchBookController(IPatchBookUseCase patchBookUseCase, IBookDtoMapper mapper) : IPatchBookController
{
    public async Task<IResult> PatchBook(string id, PatchBookRequestDto requestDto)
    {
        Book book = await patchBookUseCase.Patch(new PatchBookCommand(
            id,
            requestDto.Title,
            requestDto.AuthorId,
            requestDto.Description,
            requestDto.Keywords?.ToArray()));

        return Results.Ok(mapper.ToDto(book));
    }
}
