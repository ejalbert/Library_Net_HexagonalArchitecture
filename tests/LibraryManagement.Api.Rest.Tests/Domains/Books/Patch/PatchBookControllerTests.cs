using System.Linq;
using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Api.Rest.Client.Domain.Books.Patch;
using LibraryManagement.Api.Rest.Domains.Books;
using LibraryManagement.Api.Rest.Domains.Books.PatchBook;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Patch;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace LibraryManagement.Api.Rest.Tests.Domains.Books.Patch;

public class PatchBookControllerTests
{
    [Fact]
    public async Task PatchBook_ReturnsOkWithPatchedBook()
    {
        Book patched = new()
        {
            Id = "book-id",
            Title = "The Hobbit",
            AuthorId = "author-1",
            Description = "Updated",
            Keywords = ["fantasy"]
        };

        Mock<IPatchBookUseCase> useCaseMock = new();
        useCaseMock
            .Setup(x => x.Patch(It.IsAny<PatchBookCommand>()))
            .ReturnsAsync(patched);

        Mock<IBookDtoMapper> mapperMock = new();
        mapperMock.Setup(mapper => mapper.ToDto(patched)).Returns(new BookDto
        {
            Id = patched.Id,
            Title = patched.Title,
            AuthorId = patched.AuthorId,
            Description = patched.Description,
            Keywords = patched.Keywords
        });

        PatchBookController controller = new(useCaseMock.Object, mapperMock.Object);
        PatchBookRequestDto request = new(Description: "Updated");

        IResult result = await controller.PatchBook("book-id", request);

        var okResult = Assert.IsType<Ok<BookDto>>(result);
        Assert.Equal(patched.Description, okResult.Value!.Description);
        useCaseMock.Verify(x => x.Patch(It.Is<PatchBookCommand>(command =>
            command == new PatchBookCommand("book-id", null, null, request.Description))), Times.Once);
    }
}
