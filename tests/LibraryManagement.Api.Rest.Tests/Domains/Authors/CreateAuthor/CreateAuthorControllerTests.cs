using LibraryManagement.Api.Rest.Client.Domain.Authors;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Create;
using LibraryManagement.Api.Rest.Domains.Authors;
using LibraryManagement.Api.Rest.Domains.Authors.CreateAuthor;
using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Domain.Domains.Authors.Create;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

namespace LibraryManagement.Api.Rest.Tests.Domains.Authors.CreateAuthor;

public class CreateAuthorControllerTests
{
    [Fact]
    public async Task CreateAuthor_returns_mapped_author()
    {
        Author author = new() { Id = "author-5", Name = "Martin Fowler" };
        AuthorDto dto = new() { Id = author.Id, Name = author.Name };

        Mock<ICreateAuthorUseCase> useCaseMock = new();
        useCaseMock.Setup(useCase => useCase.Create(It.IsAny<CreateAuthorCommand>()))
            .ReturnsAsync(author);

        Mock<IAuthorDtoMapper> mapperMock = new();
        mapperMock.Setup(mapper => mapper.ToDto(author)).Returns(dto);

        CreateAuthorController controller = new(useCaseMock.Object, mapperMock.Object);

        IResult result = await controller.CreateAuthor(new CreateAuthorRequestDto(author.Name));

        Ok<AuthorDto> okResult = Assert.IsType<Ok<AuthorDto>>(result);
        Assert.Equal(dto, okResult.Value);

        useCaseMock.Verify(useCase => useCase.Create(
            It.Is<CreateAuthorCommand>(command => command == new CreateAuthorCommand(author.Name))),
            Times.Once);
    }
}
