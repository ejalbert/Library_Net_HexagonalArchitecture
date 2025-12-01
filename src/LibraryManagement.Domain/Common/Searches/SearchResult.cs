namespace LibraryManagement.Domain.Common.Searches;

public class SearchResult<TResult>
{

    public IEnumerable<TResult> Results { get; init; }

    public PaginationInfo Pagination { get; init; }
}
