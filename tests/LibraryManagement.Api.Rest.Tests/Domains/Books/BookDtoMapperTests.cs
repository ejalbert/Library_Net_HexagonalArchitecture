using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Api.Rest.Domains.Books;
using LibraryManagement.Domain.Domains.Books;

namespace LibraryManagement.Api.Rest.Tests.Domains.Books;

public class BookDtoMapperTests
{
    [Fact]
    public void ToDto_MapsDomainBookToDto()
    {
        var mapper = new BookDtoMapper();
        var book = new Book
        {
            Id = "book-id",
            Title = "The Hobbit",
            AuthorId = "author-1",
            Description = "A journey",
            Keywords = new[] { "fantasy", "adventure" }
        };

        BookDto dto = mapper.ToDto(book);

        Assert.Equal(book.Id, dto.Id);
        Assert.Equal(book.Title, dto.Title);
        Assert.Equal(book.AuthorId, dto.AuthorId);
        Assert.Equal(book.Description, dto.Description);
        Assert.Equal(book.Keywords, dto.Keywords);
    }
}
