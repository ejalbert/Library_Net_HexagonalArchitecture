using System.Linq;

using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Api.Rest.Client.Domain.Books.Create;
using LibraryManagement.Api.Rest.Domains.Books;
using LibraryManagement.Api.Rest.Domains.Books.CreateNewBook;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Create;

using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

namespace LibraryManagement.Api.Rest.Tests.Domains.Books;

public class CreateNewBookControllerTests
{
    [Fact]
    public async Task CreateNewBook_ReturnsOkWithBookFromUseCase()
    {
        var book = new Book
        {
            Id = "book-id",
            Title = "The Hobbit",
            AuthorId = "author-1",
            Description = "A journey",
            Keywords = ["fantasy", "adventure"]
        };
        var expected = new BookDto()
        {
            Id = book.Id,
            Title = book.Title,
            AuthorId = book.AuthorId,
            Description = book.Description,
            Keywords = book.Keywords
        };
        var useCaseMock = new Mock<ICreateNewBookUseCase>();
        useCaseMock
            .Setup(x => x.Create(It.IsAny<CreateNewBookCommand>()))
            .ReturnsAsync(()=>book);

        var mapperMock = new Mock<IBookDtoMapper>();
        mapperMock.Setup(x => x.ToDto(It.IsAny<Book>())).Returns(new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            AuthorId = book.AuthorId,
            Description = book.Description,
            Keywords = book.Keywords
        });

        var controller = new CreateNewBookController(useCaseMock.Object, mapperMock.Object);
        var request = new CreateNewBookRequestDto("The Hobbit", "author-1", "A journey", ["fantasy", "adventure"]);

        var result = await controller.CreateNewBook(request);

        var okResult = Assert.IsType<Ok<BookDto>>(result);
        Assert.Equivalent(expected, okResult.Value);
        useCaseMock.Verify(
            x => x.Create(It.IsAny<CreateNewBookCommand>()),
            Times.Once);
    }
}
