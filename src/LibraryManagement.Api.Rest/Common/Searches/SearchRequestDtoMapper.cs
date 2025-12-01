using LibraryManagement.Api.Rest.Client.Common.Searches;
using LibraryManagement.Domain.Common.Searches;

using Riok.Mapperly.Abstractions;

namespace LibraryManagement.Api.Rest.Common.Searches;

public interface ISearchDtoMapper
{
    Pagination ToDomain(PaginationDto paginationDto);
    PaginationInfoDto ToDto(PaginationInfo paginationInfo);
}

[Mapper]
public partial class SearchDtoMapper : ISearchDtoMapper
{
    public partial Pagination ToDomain(PaginationDto paginationDto);

    public partial PaginationInfoDto ToDto(PaginationInfo paginationInfo);
}
