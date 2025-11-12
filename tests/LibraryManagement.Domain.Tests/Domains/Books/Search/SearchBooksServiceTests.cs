using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Search;

using Moq;

namespace LibraryManagement.Domain.Tests.Domains.Books.Search;

public class SearchBooksServiceTests
{
    [Fact]
    public async Task Search_returns_books_from_port()
    {
        Mock<ISearchBooksPort> portMock = new();
        IEnumerable<Book> expected = new[]
        {
            new Book { Id = "book-1", Title = "Clean Code", AuthorId = "author-1", Description = "A guide to clean code", Keywords = new[] { "clean-code" } },
            new Book { Id = "book-2", Title = "Domain-Driven Design", AuthorId = "author-2", Description = "DDD fundamentals", Keywords = new[] { "ddd" } }
        };

        portMock.Setup(port => port.Search("code"))
            .ReturnsAsync(expected);

        SearchBooksService service = new(portMock.Object);

        IEnumerable<Book> result = await service.Search(new SearchBooksCommand("code"));

        Assert.Same(expected, result);
        portMock.Verify(port => port.Search("code"), Times.Once);
    }
}
