using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Update;

using Microsoft.Extensions.Logging;

using Moq;

namespace LibraryManagement.Domain.Tests.Domains.Books.Update;

public class UpdateBookServiceTests
{
    [Fact]
    public async Task Update_passes_id_and_title_to_port_and_returns_updated_book()
    {
        Mock<IUpdateBookPort> portMock = new();
        Mock<ILogger<UpdateBookService>> loggerMock = new();
        Book updated = new() { Id = "book-123", Title = "Clean Architecture (2nd Ed.)" };

        portMock.Setup(port => port.Update("book-123", "Clean Architecture (2nd Ed.)"))
            .ReturnsAsync(updated);

        UpdateBookService service = new(portMock.Object, loggerMock.Object);

        Book result = await service.Update(new UpdateBookCommand("book-123", "Clean Architecture (2nd Ed.)"));

        Assert.Same(updated, result);
        portMock.Verify(port => port.Update("book-123", "Clean Architecture (2nd Ed.)"), Times.Once);
    }
}
