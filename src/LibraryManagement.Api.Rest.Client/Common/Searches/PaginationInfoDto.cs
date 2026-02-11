using System.ComponentModel;

namespace LibraryManagement.Api.Rest.Client.Common.Searches;

[Description("Pagination info DTO")]
public record PaginationInfoDto(
    [property: Description("Current page index")]
    int PageIndex,
    [property: Description("Current page size")]
    int PageSize,
    [property: Description("Total number of available items")]
    long TotalItems);
