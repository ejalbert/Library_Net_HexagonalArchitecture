using System.ComponentModel;

namespace LibraryManagement.Api.Rest.Client.Common.Searches;

[Description("Base DTO for search requests")]
public abstract record SearchRequestDtoBase(
    [property: Description("Search term to filter results")]
    String? SearchTerm,
    [property: Description("Pagination options for the search")]
    PaginationDto? Pagination);
