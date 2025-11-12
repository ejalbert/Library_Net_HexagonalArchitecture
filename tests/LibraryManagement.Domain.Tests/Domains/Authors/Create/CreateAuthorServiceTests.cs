using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Domain.Domains.Authors.Create;

using Microsoft.Extensions.Logging;

using Moq;

namespace LibraryManagement.Domain.Tests.Domains.Authors.Create;

public class CreateAuthorServiceTests
{
    [Fact]
    public async Task Create_passes_name_to_port_and_returns_created_author()
    {
        Mock<ICreateAuthorPort> portMock = new();
        Mock<ILogger<CreateAuthorService>> loggerMock = new();
        Author persisted = new() { Id = "author-123", Name = "Robert C. Martin" };

        portMock.Setup(port => port.Create("Robert C. Martin"))
            .ReturnsAsync(persisted);

        CreateAuthorService service = new(portMock.Object, loggerMock.Object);

        Author result = await service.Create(new CreateAuthorCommand("Robert C. Martin"));

        Assert.Same(persisted, result);
        portMock.Verify(port => port.Create("Robert C. Martin"), Times.Once);
    }
}
