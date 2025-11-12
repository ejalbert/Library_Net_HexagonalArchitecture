using System.Linq;

using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Create;

using Microsoft.Extensions.Logging;

using Moq;

namespace LibraryManagement.Domain.Tests.Domains.Books.Create;

public class CreateNewBookServiceTests
{
    [Fact]
    public async Task Create_passes_all_fields_to_port_and_returns_created_book()
    {
        Mock<ICreateNewBookPort> portMock = new();
        Mock<ILogger<CreateNewBookService>> loggerMock = new();
        Book persisted = new()
        {
            Id = "book-123",
            Title = "Clean Architecture",
            AuthorId = "author-1",
            Description = "Architectural patterns",
            Keywords = new[] { "architecture", "clean-code" }
        };

        portMock.Setup(port => port.Create("Clean Architecture", "author-1", "Architectural patterns", It.Is<IReadOnlyCollection<string>>(k => k.SequenceEqual(new[] { "architecture", "clean-code" }))))
            .ReturnsAsync(persisted);

        CreateNewBookService service = new(portMock.Object, loggerMock.Object);

        Book result = await service.Create(new CreateNewBookCommand("Clean Architecture", "author-1", "Architectural patterns", new[] { "architecture", "clean-code" }));

        Assert.Same(persisted, result);
        portMock.Verify(port => port.Create("Clean Architecture", "author-1", "Architectural patterns", It.Is<IReadOnlyCollection<string>>(k => k.SequenceEqual(new[] { "architecture", "clean-code" }))), Times.Once);
    }
}
