using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.GetSingle;
using Moq;

namespace LibraryManagement.Domain.Tests.Domains.Books.GetSingle;

public class GetSingleBookServiceTests
{
    [Fact]
    public async Task Get_returns_book_retrieved_from_port()
    {
        Mock<IGetSingleBookPort> portMock = new();
        Book expected = new() { Id = "book-42", Title = "Refactoring" };

        portMock.Setup(port => port.GetById("book-42"))
            .ReturnsAsync(expected);

        GetSingleBookService service = new(portMock.Object);

        Book result = await service.Get(new GetSingleBookCommand("book-42"));

        Assert.Same(expected, result);
        portMock.Verify(port => port.GetById("book-42"), Times.Once);
    }
}
