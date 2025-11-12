using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;
using LibraryManagement.Api.Rest.Domains.Books;
using LibraryManagement.Api.Rest.Domains.Books.Search;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Search;

using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

namespace LibraryManagement.Api.Rest.Tests.Domains.Books;

public class SearchBooksControllerTests
{
    [Fact]
    public async Task SearchBooks_ReturnsMappedDtosInOkResult()
    {
        var bookOne = new Book { Id = "book-1", Title = "The Hobbit", AuthorId = "author-1" };
        var bookTwo = new Book { Id = "book-2", Title = "The Silmarillion", AuthorId = "author-2" };
        var books = new[] { bookOne, bookTwo };
        var responseDtoOne = new BookDto { Id = "book-1", Title = "The Hobbit", AuthorId = "author-1" };
        var responseDtoTwo = new BookDto { Id = "book-2", Title = "The Silmarillion", AuthorId = "author-2" };
        var request = new SearchBooksRequestDto("hobbit");
        var useCaseMock = new Mock<ISearchBooksUseCase>();
        useCaseMock
            .Setup(x => x.Search(It.IsAny<SearchBooksCommand>()))
            .ReturnsAsync(books);
        var mapperMock = new Mock<IBookDtoMapper>();
        mapperMock.Setup(x => x.ToDto(bookOne)).Returns(responseDtoOne);
        mapperMock.Setup(x => x.ToDto(bookTwo)).Returns(responseDtoTwo);
        var controller = new SearchBooksController(useCaseMock.Object, mapperMock.Object);

        var result = await controller.SearchBooks(request);

        var okResult = Assert.IsType<Ok<SearchBooksResponseDto>>(result);
        var payload = Assert.IsType<SearchBooksResponseDto>(okResult.Value);
        Assert.Collection(
            payload.Books,
            dto => Assert.Same(responseDtoOne, dto),
            dto => Assert.Same(responseDtoTwo, dto));
        useCaseMock.Verify(
            x => x.Search(It.Is<SearchBooksCommand>(command => command == new SearchBooksCommand(request.SearchTerm))),
            Times.Once);
        mapperMock.Verify(x => x.ToDto(bookOne), Times.Once);
        mapperMock.Verify(x => x.ToDto(bookTwo), Times.Once);
    }
}
