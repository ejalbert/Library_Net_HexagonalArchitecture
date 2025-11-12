using LibraryManagement.Api.Rest.Client.Domain.Books.Update;

using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Domains.Books.UpdateBook;

public interface IUpdateBookController
{
    Task<IResult> UpdateBook(string id, UpdateBookRequestDto requestDto);
}
