using LibraryManagement.Api.Rest.Domains.Books.GetSingleBook;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.GetSingle;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

namespace LibraryManagement.Api.Rest.Tests.Domains.Books;

public class GetBookControllerTests
{
    [Fact]
    public async Task GetBookById_ReturnsOkWithBookFromUseCase()
    {
        var book = new Book
        {
            Id = "book-id",
            Title = "The Hobbit",
            AuthorId = "author-1",
            Description = "A journey",
            Keywords = new[] { "fantasy", "adventure" }
        };
        var useCaseMock = new Mock<IGetSingleBookUseCase>();
        useCaseMock
            .Setup(x => x.Get(It.IsAny<GetSingleBookCommand>()))
            .ReturnsAsync(book);
        var controller = new GetBookController(useCaseMock.Object);

        IResult result = await controller.GetBookById(book.Id);

        Ok<Book> okResult = Assert.IsType<Ok<Book>>(result);
        Assert.Same(book, okResult.Value);
        useCaseMock.Verify(
            x => x.Get(It.Is<GetSingleBookCommand>(command => command == new GetSingleBookCommand(book.Id))),
            Times.Once);
    }
}
