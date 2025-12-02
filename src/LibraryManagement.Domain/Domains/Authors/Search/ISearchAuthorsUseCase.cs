using LibraryManagement.Domain.Common.Searches;

namespace LibraryManagement.Domain.Domains.Authors.Search;

public interface ISearchAuthorsUseCase
{
    Task<SearchResult<Author>> Search(SearchAuthorsCommand command);
}
