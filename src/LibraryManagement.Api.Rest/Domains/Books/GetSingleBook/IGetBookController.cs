using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Domains.Books.GetSingleBook;

public interface IGetBookController
{
    Task<IResult> GetBookById(string id);
}