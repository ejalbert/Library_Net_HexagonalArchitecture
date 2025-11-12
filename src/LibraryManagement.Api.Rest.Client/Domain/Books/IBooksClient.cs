using LibraryManagement.Api.Rest.Client.Domain.Books.Create;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;
using LibraryManagement.Api.Rest.Client.Domain.Books.Update;
using LibraryManagement.Api.Rest.Client.Domain.Books.Patch;

namespace LibraryManagement.Api.Rest.Client.Domain.Books;

public interface IBooksClient
{
    Task<BookDto> Create(CreateNewBookRequestDto requestDto, CancellationToken cancellationToken = default);

    Task<BookDto> Get(string bookId, CancellationToken cancellationToken = default);

    Task<BookDto> Update(string bookId, UpdateBookRequestDto requestDto, CancellationToken cancellationToken = default);

    Task<BookDto> Patch(string bookId, PatchBookRequestDto requestDto, CancellationToken cancellationToken = default);

    Task<SearchBooksResponseDto> Search(SearchBooksRequestDto requestDto, CancellationToken cancellationToken = default);

    Task Delete(string bookId, CancellationToken cancellationToken = default);
}
