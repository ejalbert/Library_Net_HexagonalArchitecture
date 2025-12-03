namespace LibraryManagement.Api.Rest.Client.Common.Searches;

public abstract record SearchRequestDtoBase(String? SearchTerm, PaginationDto? Pagination);
