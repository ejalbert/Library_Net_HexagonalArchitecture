using System.Linq;

using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Api.Rest.Client.Domain.Books.Update;
using LibraryManagement.Api.Rest.Domains.Books;
using LibraryManagement.Api.Rest.Domains.Books.UpdateBook;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Update;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

namespace LibraryManagement.Api.Rest.Tests.Domains.Books;

public class UpdateBookControllerTests
{
    [Fact]
    public async Task UpdateBook_ReturnsOkWithUpdatedBook()
    {
        Book updated = new()
        {
            Id = "book-id",
            Title = "The Hobbit - Revised",
            AuthorId = "author-1",
            Description = "Updated journey",
            Keywords = new[] { "fantasy", "revised" }
        };
        var useCaseMock = new Mock<IUpdateBookUseCase>();
        useCaseMock
            .Setup(useCase => useCase.Update(It.IsAny<UpdateBookCommand>()))
            .ReturnsAsync(updated);

        BookDto mapped = new()
        {
            Id = updated.Id,
            Title = updated.Title,
            AuthorId = updated.AuthorId,
            Description = updated.Description,
            Keywords = updated.Keywords
        };
        var mapperMock = new Mock<IBookDtoMapper>();
        mapperMock.Setup(mapper => mapper.ToDto(updated)).Returns(mapped);

        UpdateBookController controller = new(useCaseMock.Object, mapperMock.Object);
        UpdateBookRequestDto request = new("The Hobbit - Revised", "author-1", "Updated journey", new[] { "fantasy", "revised" });

        IResult result = await controller.UpdateBook("book-id", request);

        var okResult = Assert.IsType<Ok<BookDto>>(result);
        Assert.Equal(mapped, okResult.Value);
        useCaseMock.Verify(useCase =>
                useCase.Update(It.IsAny<UpdateBookCommand>()),
            Times.Once);
    }
}
