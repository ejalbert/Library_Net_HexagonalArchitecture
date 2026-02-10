using System.ComponentModel;

namespace LibraryManagement.Api.Rest.Client.Common.Searches;

[Description("Base DTO for search responses")]
public abstract record SearchResponseDtoBase<TResult>(
    [property: Description("Search results")]
    IEnumerable<TResult> Results,
    [property: Description("Pagination info for the results")]
    PaginationInfoDto Pagination);
