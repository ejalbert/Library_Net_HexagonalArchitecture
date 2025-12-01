namespace LibraryManagement.Domain.Common.Searches;

public class PaginationInfo
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public long TotalItems { get; init; }
}
