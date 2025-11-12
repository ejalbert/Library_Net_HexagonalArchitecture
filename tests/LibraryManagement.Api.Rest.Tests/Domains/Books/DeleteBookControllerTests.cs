using LibraryManagement.Api.Rest.Domains.Books.DeleteBook;
using LibraryManagement.Domain.Domains.Books.Delete;

using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

namespace LibraryManagement.Api.Rest.Tests.Domains.Books;

public class DeleteBookControllerTests
{
    [Fact]
    public async Task DeleteBook_ReturnsNoContent()
    {
        var useCaseMock = new Mock<IDeleteBookUseCase>();
        useCaseMock
            .Setup(useCase => useCase.Delete(It.IsAny<DeleteBookCommand>()))
            .Returns(Task.CompletedTask);
        DeleteBookController controller = new(useCaseMock.Object);

        var result = await controller.DeleteBook("book-1");

        Assert.IsType<NoContent>(result);
        useCaseMock.Verify(useCase => useCase.Delete(
            It.Is<DeleteBookCommand>(command => command == new DeleteBookCommand("book-1"))), Times.Once);
    }
}
