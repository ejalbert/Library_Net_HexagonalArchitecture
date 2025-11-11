using LibraryManagement.Api.Rest.Client.Domain.Books.Create;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;

namespace LibraryManagement.Api.Rest.Client.Domain.Books;

public interface IBooksClient
{
    Task<BookDto> Create(CreateNewBookRequestDto requestDto, CancellationToken cancellationToken = default);

    Task<BookDto> Get(string bookId, CancellationToken cancellationToken = default);

    Task<SearchBooksResponseDto> Search(SearchBooksRequestDto requestDto, CancellationToken cancellationToken = default);
}
