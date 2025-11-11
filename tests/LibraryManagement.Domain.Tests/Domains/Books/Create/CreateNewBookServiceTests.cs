using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Create;

using Microsoft.Extensions.Logging;

using Moq;

namespace LibraryManagement.Domain.Tests.Domains.Books.Create;

public class CreateNewBookServiceTests
{
    [Fact]
    public async Task Create_passes_title_to_port_and_returns_created_book()
    {
        Mock<ICreateNewBookPort> portMock = new();
        Mock<ILogger<CreateNewBookService>> loggerMock = new();
        Book persisted = new() { Id = "book-123", Title = "Clean Architecture" };

        portMock.Setup(port => port.Create("Clean Architecture"))
            .ReturnsAsync(persisted);

        CreateNewBookService service = new(portMock.Object, loggerMock.Object);

        Book result = await service.Create(new CreateNewBookCommand("Clean Architecture"));

        Assert.Same(persisted, result);
        portMock.Verify(port => port.Create("Clean Architecture"), Times.Once);
    }
}
