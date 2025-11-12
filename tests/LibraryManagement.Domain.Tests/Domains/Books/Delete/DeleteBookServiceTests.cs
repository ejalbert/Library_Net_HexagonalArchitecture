using LibraryManagement.Domain.Domains.Books.Delete;

using Microsoft.Extensions.Logging;

using Moq;

namespace LibraryManagement.Domain.Tests.Domains.Books.Delete;

public class DeleteBookServiceTests
{
    [Fact]
    public async Task Delete_passes_id_to_port()
    {
        Mock<IDeleteBookPort> portMock = new();
        Mock<ILogger<DeleteBookService>> loggerMock = new();
        DeleteBookService service = new(portMock.Object, loggerMock.Object);

        await service.Delete(new DeleteBookCommand("book-42"));

        portMock.Verify(port => port.Delete("book-42"), Times.Once);
    }
}
