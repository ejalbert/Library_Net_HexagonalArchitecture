namespace LibraryManagement.Api.Rest.Client.Common.Searches;

public abstract record SearchResponseDtoBase<TResult>(IEnumerable<TResult> Results, PaginationInfoDto Pagination);
