using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Domain.Domains.Books;

namespace LibraryManagement.Api.Rest.Domains.Books;

public interface IBookDtoMapper
{
    public BookDto ToDto(Book book);
}