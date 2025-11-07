using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Domains.Books.CreateNewBook;

public interface ICreateNewBookController
{
    Task<IResult> CreateNewBook(CreateNewBookRequestDto requestDto);
}