using LibraryManagement.Api.Rest.Client.Domain.Books.Patch;

using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Domains.Books.PatchBook;

public interface IPatchBookController
{
    Task<IResult> PatchBook(string id, PatchBookRequestDto requestDto);
}
