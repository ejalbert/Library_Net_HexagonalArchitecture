using System.ComponentModel;

using LibraryManagement.Api.Rest.Client.Domain.Books.Create;
using LibraryManagement.Api.Rest.Client.Domain.Books.Patch;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;
using LibraryManagement.Api.Rest.Client.Domain.Books.Update;

namespace LibraryManagement.Api.Rest.Client.Domain.Books;

public interface IBooksClient
{
    Task<BookDto> Create(CreateNewBookRequestDto requestDto, CancellationToken cancellationToken = default);

    Task<BookDto> Get([Description("Book identifier")] string bookId,
        CancellationToken cancellationToken = default);

    Task<BookDto> Update([Description("Book identifier")] string bookId, UpdateBookRequestDto requestDto,
        CancellationToken cancellationToken = default);

    Task<BookDto> Patch([Description("Book identifier")] string bookId, PatchBookRequestDto requestDto,
        CancellationToken cancellationToken = default);

    Task<SearchBooksResponseDto> Search(SearchBooksRequestDto requestDto,
        CancellationToken cancellationToken = default);

    Task Delete([Description("Book identifier")] string bookId, CancellationToken cancellationToken = default);
}
