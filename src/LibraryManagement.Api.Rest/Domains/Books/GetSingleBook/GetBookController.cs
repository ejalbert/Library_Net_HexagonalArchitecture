using LibraryManagement.Domain.Domains.Books.GetSingle;

using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Domains.Books.GetSingleBook;

public class GetBookController(IGetSingleBookUseCase getSingleBookUseCase) : IGetBookController
{
    public async Task<IResult> GetBookById(string id)
    {
        var book = await getSingleBookUseCase.Get(
            new GetSingleBookCommand(id)
        );

        return Results.Ok(book);
    }
}
