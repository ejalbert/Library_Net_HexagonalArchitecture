using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Domain.Domains.Books;

using Riok.Mapperly.Abstractions;

namespace LibraryManagement.Api.Rest.Domains.Books;

[Mapper]
public partial class BookDtoMapper : IBookDtoMapper
{
    public partial BookDto ToDto(Book book);
}
