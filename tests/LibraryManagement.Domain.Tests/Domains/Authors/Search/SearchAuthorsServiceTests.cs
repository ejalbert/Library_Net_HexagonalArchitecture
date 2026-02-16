using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Domain.Domains.Authors.Search;

using Moq;

namespace LibraryManagement.Domain.Tests.Domains.Authors.Search;

public class SearchAuthorsServiceTests
{
    [Fact]
    public async Task Search_returns_authors_from_port()
    {
        Mock<ISearchAuthorsPort> portMock = new();
        SearchResult<Author> expected = new()
        {
            Results =
            [
                new Author { Id = "author-1", Name = "Kent Beck" },
                new Author { Id = "author-2", Name = "Martin Fowler" }
            ],
            Pagination = new PaginationInfo
            {
                TotalItems = 2,
                PageIndex = 0,
                PageSize = 10
            }
        };

        portMock.Setup(port => port.Search("martin", new Pagination(0, 10)))
            .ReturnsAsync(expected);

        SearchAuthorsService service = new(portMock.Object);

        SearchResult<Author> result =
            await service.Search(new SearchAuthorsCommand("martin", new Pagination(0, 10)));

        Assert.Same(expected, result);
        portMock.Verify(port => port.Search("martin", new Pagination(0, 10)), Times.Once);
    }
}
