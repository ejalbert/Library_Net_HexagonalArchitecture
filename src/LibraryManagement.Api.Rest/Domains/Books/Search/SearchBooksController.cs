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

public class SearchBooksController(
    ISearchBooksUseCase searchBooksUseCase,
    IBookDtoMapper mapper,
    ISearchDtoMapper searchDtoMapper)
    : ISearchBooksController
{
    public async Task<IResult> SearchBooks(SearchBooksRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        Pagination? pagination = requestDto.Pagination != null ? searchDtoMapper.ToDomain(requestDto.Pagination) : null;

        SearchResult<Book> searchResult =
            await searchBooksUseCase.Search(new SearchBooksCommand(requestDto.SearchTerm, pagination));

        return Results.Ok(new SearchBooksResponseDto(searchResult.Results.Select(mapper.ToDto),
            searchDtoMapper.ToDto(searchResult.Pagination)));
    }
}
