using LibraryManagement.Api.Rest.Client.Common.Searches;
using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;
using LibraryManagement.Api.Rest.Common.Searches;
using LibraryManagement.Api.Rest.Domains.Books;
using LibraryManagement.Api.Rest.Domains.Books.Search;
using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Search;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

namespace LibraryManagement.Api.Rest.Tests.Domains.Books;

public class SearchBooksControllerTests
{
    [Fact]
    public async Task SearchBooks_ReturnsMappedDtosInOkResult()
    {
        var bookOne = new Book
        {
            Id = "book-1",
            Title = "The Hobbit",
            AuthorId = "author-1",
            Description = "A hobbit tale",
            Keywords = new[] { "fantasy" }
        };
        var bookTwo = new Book
        {
            Id = "book-2",
            Title = "The Silmarillion",
            AuthorId = "author-2",
            Description = "Lore of Middle-earth",
            Keywords = new[] { "fantasy", "lore" }
        };
        Book[] books = new[] { bookOne, bookTwo };
        var responseDtoOne = new BookDto
        {
            Id = "book-1",
            Title = "The Hobbit",
            AuthorId = "author-1",
            Description = "A hobbit tale",
            Keywords = new[] { "fantasy" }
        };
        var responseDtoTwo = new BookDto
        {
            Id = "book-2",
            Title = "The Silmarillion",
            AuthorId = "author-2",
            Description = "Lore of Middle-earth",
            Keywords = new[] { "fantasy", "lore" }
        };
        var request = new SearchBooksRequestDto("hobbit", new PaginationDto(0, 10));
        var useCaseMock = new Mock<ISearchBooksUseCase>();
        useCaseMock
            .Setup(x => x.Search(It.IsAny<SearchBooksCommand>()))
            .ReturnsAsync(new SearchResult<Book> { Results = books, Pagination = new() { TotalItems = 2, PageIndex = 0, PageSize = 10 } });
        var bookMapperMock = new Mock<IBookDtoMapper>();
        bookMapperMock.Setup(x => x.ToDto(bookOne)).Returns(responseDtoOne);
        bookMapperMock.Setup(x => x.ToDto(bookTwo)).Returns(responseDtoTwo);

        var searchRequestDtoMapperMock = new Mock<ISearchDtoMapper>();


        var controller = new SearchBooksController(useCaseMock.Object, bookMapperMock.Object, searchRequestDtoMapperMock.Object);

        IResult result = await controller.SearchBooks(request);

        Ok<SearchBooksResponseDto> okResult = Assert.IsType<Ok<SearchBooksResponseDto>>(result);
        SearchBooksResponseDto payload = Assert.IsType<SearchBooksResponseDto>(okResult.Value);
        Assert.Collection(
            payload.Results,
            dto => Assert.Same(responseDtoOne, dto),
            dto => Assert.Same(responseDtoTwo, dto));
        useCaseMock.Verify(
            x => x.Search(It.Is<SearchBooksCommand>(command => command == new SearchBooksCommand(request.SearchTerm, null))),
            Times.Once);
        bookMapperMock.Verify(x => x.ToDto(bookOne), Times.Once);
        bookMapperMock.Verify(x => x.ToDto(bookTwo), Times.Once);
    }
}
