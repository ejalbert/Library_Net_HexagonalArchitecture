using LibraryManagement.Domain.Common.Searches;

namespace LibraryManagement.Domain.Domains.Authors.Search;

public class SearchAuthorsService(ISearchAuthorsPort searchAuthorsPort) : ISearchAuthorsUseCase
{
    public Task<SearchResult<Author>> Search(SearchAuthorsCommand command)
    {
        return searchAuthorsPort.Search(command.SearchTerm, command.Pagination ?? new Pagination(0, 10));
    }
}
