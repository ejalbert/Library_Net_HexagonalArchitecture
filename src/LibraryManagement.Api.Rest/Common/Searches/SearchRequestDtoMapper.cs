using LibraryManagement.Api.Rest.Client.Common.Searches;
using LibraryManagement.Domain.Common.Searches;

using Riok.Mapperly.Abstractions;

namespace LibraryManagement.Api.Rest.Common.Searches;

public interface ISearchRequestDtoMapper
{
    Pagination ToDomain(PaginationDto paginationDto);
}

[Mapper]
public partial class SearchRequestDtoMapper : ISearchRequestDtoMapper
{
    public partial Pagination ToDomain(PaginationDto paginationDto);
}
