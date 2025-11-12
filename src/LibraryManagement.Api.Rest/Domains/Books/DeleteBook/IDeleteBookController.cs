using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Domains.Books.DeleteBook;

public interface IDeleteBookController
{
    Task<IResult> DeleteBook(string id);
}
