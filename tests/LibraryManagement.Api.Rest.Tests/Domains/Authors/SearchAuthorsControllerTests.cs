using LibraryManagement.Api.Rest.Client.Common.Searches;
using LibraryManagement.Api.Rest.Client.Domain.Authors;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Search;
using LibraryManagement.Api.Rest.Common.Searches;
using LibraryManagement.Api.Rest.Domains.Authors;
using LibraryManagement.Api.Rest.Domains.Authors.Search;
using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Domain.Domains.Authors.Search;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

namespace LibraryManagement.Api.Rest.Tests.Domains.Authors;

public class SearchAuthorsControllerTests
{
    [Fact]
    public async Task SearchAuthors_ReturnsMappedDtosInOkResult()
    {
        Author authorOne = new() { Id = "author-1", Name = "J.K. Rowling" };
        Author authorTwo = new() { Id = "author-2", Name = "J.R.R. Tolkien" };
        var responseDtoOne = new AuthorDto { Id = "author-1", Name = "J.K. Rowling" };
        var responseDtoTwo = new AuthorDto { Id = "author-2", Name = "J.R.R. Tolkien" };
        var request = new SearchAuthorsRequestDto("Rowling", new PaginationDto(0, 10));
        Mock<ISearchAuthorsUseCase> useCaseMock = new();
        useCaseMock
            .Setup(x => x.Search(It.IsAny<SearchAuthorsCommand>()))
            .ReturnsAsync(new SearchResult<Author>
            {
                Results = new[] { authorOne, authorTwo },
                Pagination = new PaginationInfo { TotalItems = 2, PageIndex = 0, PageSize = 10 }
            });
        Mock<IAuthorDtoMapper> mapperMock = new();
        mapperMock.Setup(x => x.ToDto(authorOne)).Returns(responseDtoOne);
        mapperMock.Setup(x => x.ToDto(authorTwo)).Returns(responseDtoTwo);

        Mock<ISearchDtoMapper> searchDtoMapperMock = new();

        var controller = new SearchAuthorsController(useCaseMock.Object, mapperMock.Object, searchDtoMapperMock.Object);

        IResult result = await controller.SearchAuthors(request);

        Ok<SearchAuthorsResponseDto> okResult = Assert.IsType<Ok<SearchAuthorsResponseDto>>(result);
        SearchAuthorsResponseDto payload = Assert.IsType<SearchAuthorsResponseDto>(okResult.Value);
        Assert.Collection(
            payload.Results,
            dto => Assert.Same(responseDtoOne, dto),
            dto => Assert.Same(responseDtoTwo, dto));
        useCaseMock.Verify(
            x => x.Search(It.Is<SearchAuthorsCommand>(command =>
                command == new SearchAuthorsCommand(request.SearchTerm, null))),
            Times.Once);
        mapperMock.Verify(x => x.ToDto(authorOne), Times.Once);
        mapperMock.Verify(x => x.ToDto(authorTwo), Times.Once);
    }
}
