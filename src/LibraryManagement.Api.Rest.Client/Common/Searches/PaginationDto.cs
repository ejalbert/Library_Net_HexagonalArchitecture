using System.ComponentModel;

namespace LibraryManagement.Api.Rest.Client.Common.Searches;

[Description("Pagination request DTO")]
public record PaginationDto(
    [property: Description("Page index to request")]
    int PageIndex,
    [property: Description("Page size to request")]
    int PageSize);
