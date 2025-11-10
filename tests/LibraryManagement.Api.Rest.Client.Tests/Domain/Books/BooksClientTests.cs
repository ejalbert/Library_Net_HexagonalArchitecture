using System.Net;
using System.Net.Http.Json;
using LibraryManagement.Api.Rest.Client;
using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Api.Rest.Client.Domain.Books.CreateNewBook;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;
using LibraryManagement.Api.Rest.Client.Tests.Http;

namespace LibraryManagement.Api.Rest.Client.Tests.Domain.Books;

public class BooksClientTests
{
    [Fact]
    public async Task Create_SendsPostToBooksEndpointAndReturnsCreatedBook()
    {
        var requestDto = new CreateNewBookRequestDto("The Hobbit");
        var expectedResponse = new BookDto { Id = "book-1", Title = requestDto.Title };
        var handler = new TestHttpMessageHandler(async (request, cancellationToken) =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("http://localhost/api/v1/books", request.RequestUri!.ToString());
            var payload = await request.Content!.ReadFromJsonAsync<CreateNewBookRequestDto>(cancellationToken);
            Assert.Equal(requestDto, payload);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expectedResponse)
            };
        });
        var booksClient = CreateBooksClient(handler);

        var book = await booksClient.Create(requestDto);

        Assert.Equal(expectedResponse.Id, book.Id);
        Assert.Equal(expectedResponse.Title, book.Title);
    }

    [Fact]
    public async Task Get_SendsGetRequestAndReturnsBook()
    {
        var expectedResponse = new BookDto { Id = "book-1", Title = "The Hobbit" };
        var handler = new TestHttpMessageHandler((request, _) =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal("http://localhost/api/v1/books/book-1", request.RequestUri!.ToString());

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expectedResponse)
            });
        });
        var booksClient = CreateBooksClient(handler);

        var book = await booksClient.Get("book-1");

        Assert.Equal(expectedResponse.Id, book.Id);
        Assert.Equal(expectedResponse.Title, book.Title);
    }

    [Fact]
    public async Task Search_SendsPostAndReturnsResponse()
    {
        var requestDto = new SearchBooksRequestDto("hobbit");
        var expectedResponse = new SearchBooksResponseDto(new[]
        {
            new BookDto { Id = "book-1", Title = "The Hobbit" }
        });
        var handler = new TestHttpMessageHandler(async (request, cancellationToken) =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("http://localhost/api/v1/books/search", request.RequestUri!.ToString());
            var payload = await request.Content!.ReadFromJsonAsync<SearchBooksRequestDto>(cancellationToken);
            Assert.Equal(requestDto, payload);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expectedResponse)
            };
        });
        var booksClient = CreateBooksClient(handler);

        var response = await booksClient.Search(requestDto);

        var dto = Assert.Single(response.Books);
        Assert.Equal("book-1", dto.Id);
        Assert.Equal("The Hobbit", dto.Title);
    }

    [Fact]
    public async Task Search_WhenServerReturnsError_ThrowsHttpRequestException()
    {
        var handler = new TestHttpMessageHandler((_, _) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)));
        var booksClient = CreateBooksClient(handler);

        await Assert.ThrowsAsync<HttpRequestException>(() => booksClient.Search(new SearchBooksRequestDto("hobbit")));
    }

    private static IBooksClient CreateBooksClient(HttpMessageHandler handler)
    {
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost/")
        };
        var restClient = new RestApiClient(httpClient);

        return new BooksClient(restClient);
    }
}
