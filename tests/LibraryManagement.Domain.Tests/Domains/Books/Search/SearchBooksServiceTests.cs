using LibraryManagement.Domain.Common.Searches;
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
        SearchResult<Book> expected = new()
        {
            Results =
            [
                new Book
                {
                    Id = "book-1", Title = "Clean Code", AuthorId = "author-1", Description = "A guide to clean code",
                    Keywords = new[] { "clean-code" }
                },
                new Book
                {
                    Id = "book-2", Title = "Domain-Driven Design", AuthorId = "author-2", Description = "DDD fundamentals",
                    Keywords = new[] { "ddd" }
                }
            ],
            Pagination = new()
            {
                TotalItems = 2,
                PageIndex = 0,
                PageSize = 10
            }
        };

        portMock.Setup(port => port.Search("code", new Pagination(0, 10)))
            .ReturnsAsync(expected);

        SearchBooksService service = new(portMock.Object);

        var result = await service.Search(new SearchBooksCommand("code", new Pagination(0, 10)));

        Assert.Same(expected, result);
        portMock.Verify(port => port.Search("code", new Pagination(0, 10)), Times.Once);
    }
}
