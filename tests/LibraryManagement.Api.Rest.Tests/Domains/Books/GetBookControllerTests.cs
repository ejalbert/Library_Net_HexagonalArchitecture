using LibraryManagement.Api.Rest.Domains.Books.GetSingleBook;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.GetSingle;

using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

namespace LibraryManagement.Api.Rest.Tests.Domains.Books;

public class GetBookControllerTests
{
    [Fact]
    public async Task GetBookById_ReturnsOkWithBookFromUseCase()
    {
        var book = new Book { Id = "book-id", Title = "The Hobbit" };
        var useCaseMock = new Mock<IGetSingleBookUseCase>();
        useCaseMock
            .Setup(x => x.Get(It.IsAny<GetSingleBookCommand>()))
            .ReturnsAsync(book);
        var controller = new GetBookController(useCaseMock.Object);

        var result = await controller.GetBookById(book.Id);

        var okResult = Assert.IsType<Ok<Book>>(result);
        Assert.Same(book, okResult.Value);
        useCaseMock.Verify(
            x => x.Get(It.Is<GetSingleBookCommand>(command => command == new GetSingleBookCommand(book.Id))),
            Times.Once);
    }
}
