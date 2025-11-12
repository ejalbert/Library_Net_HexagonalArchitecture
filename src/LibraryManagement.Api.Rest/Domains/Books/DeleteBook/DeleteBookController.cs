using LibraryManagement.Domain.Domains.Books.Delete;

using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Domains.Books.DeleteBook;

public class DeleteBookController(IDeleteBookUseCase deleteBookUseCase) : IDeleteBookController
{
    public async Task<IResult> DeleteBook(string id)
    {
        await deleteBookUseCase.Delete(new DeleteBookCommand(id));

        return Results.NoContent();
    }
}
