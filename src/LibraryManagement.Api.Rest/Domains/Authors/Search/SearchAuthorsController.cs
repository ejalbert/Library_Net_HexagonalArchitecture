using LibraryManagement.Api.Rest.Client.Domain.Authors.Search;
using LibraryManagement.Api.Rest.Common.Searches;
using LibraryManagement.Api.Rest.Domains.Authors;
using LibraryManagement.Domain.Domains.Authors.Search;

using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Domains.Authors.Search;

public interface ISearchAuthorsController
{
    Task<IResult> SearchAuthors(SearchAuthorsRequestDto requestDto, CancellationToken cancellationToken = default);
}

public class SearchAuthorsController(ISearchAuthorsUseCase searchAuthorsUseCase, IAuthorDtoMapper mapper,
    ISearchDtoMapper searchDtoMapper)
    : ISearchAuthorsController
{
    public async Task<IResult> SearchAuthors(SearchAuthorsRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        var pagination = requestDto.Pagination != null ? searchDtoMapper.ToDomain(requestDto.Pagination) : null;

        var searchResult =
            await searchAuthorsUseCase.Search(new SearchAuthorsCommand(requestDto.SearchTerm, pagination));

        return Results.Ok(new SearchAuthorsResponseDto(
            searchResult.Results.Select(mapper.ToDto),
            searchDtoMapper.ToDto(searchResult.Pagination)));
    }
}
