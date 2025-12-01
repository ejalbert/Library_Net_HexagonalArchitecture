using LibraryManagement.Api.Rest.Client.Common.Searches;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;
using LibraryManagement.Api.Rest.Common.Searches;
using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Search;

using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Domains.Books.Search;

public interface ISearchBooksController
{
    Task<IResult> SearchBooks(SearchBooksRequestDto requestDto, CancellationToken cancellationToken = default);
}

public class SearchBooksController(ISearchBooksUseCase searchBooksUseCase, IBookDtoMapper mapper, ISearchRequestDtoMapper searchRequestDtoMapper)
    : ISearchBooksController
{
    public async Task<IResult> SearchBooks(SearchBooksRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        var pagination = requestDto.Pagination != null ? searchRequestDtoMapper.ToDomain(requestDto.Pagination) : null;

        IEnumerable<Book> books = await searchBooksUseCase.Search(new SearchBooksCommand(requestDto.SearchTerm, pagination));

        return Results.Ok(new SearchBooksResponseDto(books.Select(mapper.ToDto)));
    }
}
