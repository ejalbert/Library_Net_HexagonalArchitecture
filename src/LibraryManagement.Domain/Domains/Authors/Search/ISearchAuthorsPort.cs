using LibraryManagement.Domain.Common.Searches;

namespace LibraryManagement.Domain.Domains.Authors.Search;

public interface ISearchAuthorsPort
{
    Task<SearchResult<Author>> Search(string? searchTerm, Pagination pagination);
}
