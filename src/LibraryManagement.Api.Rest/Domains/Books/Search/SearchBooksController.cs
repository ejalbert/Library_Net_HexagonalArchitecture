using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;
using LibraryManagement.Domain.Domains.Books.Search;
using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Domains.Books.Search;

public interface ISearchBooksController
{
    Task<IResult> SearchBooks(SearchBooksRequestDto requestDto, CancellationToken cancellationToken = default);
}

public class SearchBooksController(ISearchBooksUseCase searchBooksUseCase, IBookDtoMapper mapper) : ISearchBooksController
{
    public async Task<IResult> SearchBooks(SearchBooksRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        var books = await searchBooksUseCase.Search(new SearchBooksCommand(requestDto.SearchTerm));

        return Results.Ok(new SearchBooksResponseDto(books.Select(mapper.ToDto)));
    }
}