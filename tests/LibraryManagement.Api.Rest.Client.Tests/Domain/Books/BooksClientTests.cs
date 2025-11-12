using System.Net;
using System.Net.Http.Json;

using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Api.Rest.Client.Domain.Books.Create;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;
using LibraryManagement.Api.Rest.Client.Domain.Books.Update;
using LibraryManagement.Api.Rest.Client.Tests.Http;

namespace LibraryManagement.Api.Rest.Client.Tests.Domain.Books;

public class BooksClientTests
{
    [Fact]
    public async Task Create_SendsPostToBooksEndpointAndReturnsCreatedBook()
    {
        var requestDto = new CreateNewBookRequestDto("The Hobbit", "author-1");
        var expectedResponse = new BookDto { Id = "book-1", Title = requestDto.Title, AuthorId = requestDto.AuthorId };
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
        Assert.Equal(expectedResponse.AuthorId, book.AuthorId);
    }

    [Fact]
    public async Task Get_SendsGetRequestAndReturnsBook()
    {
        var expectedResponse = new BookDto { Id = "book-1", Title = "The Hobbit", AuthorId = "author-1" };
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
        Assert.Equal(expectedResponse.AuthorId, book.AuthorId);
    }

    [Fact]
    public async Task Search_SendsPostAndReturnsResponse()
    {
        var requestDto = new SearchBooksRequestDto("hobbit");
        var expectedResponse = new SearchBooksResponseDto(new[]
        {
            new BookDto { Id = "book-1", Title = "The Hobbit", AuthorId = "author-1" }
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
        Assert.Equal("author-1", dto.AuthorId);
    }

    [Fact]
    public async Task Search_WhenServerReturnsError_ThrowsHttpRequestException()
    {
        var handler = new TestHttpMessageHandler((_, _) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)));
        var booksClient = CreateBooksClient(handler);

        await Assert.ThrowsAsync<HttpRequestException>(() => booksClient.Search(new SearchBooksRequestDto("hobbit")));
    }

    [Fact]
    public async Task Delete_SendsDeleteRequestAndEnsuresSuccess()
    {
        var handler = new TestHttpMessageHandler((request, _) =>
        {
            Assert.Equal(HttpMethod.Delete, request.Method);
            Assert.Equal("http://localhost/api/v1/books/book-2", request.RequestUri!.ToString());

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NoContent));
        });
        var booksClient = CreateBooksClient(handler);

        await booksClient.Delete("book-2");
    }

    [Fact]
    public async Task Update_SendsPutRequestAndReturnsBook()
    {
        var requestDto = new UpdateBookRequestDto("The Hobbit - Revised", "author-2");
        var expectedResponse = new BookDto { Id = "book-1", Title = requestDto.Title, AuthorId = requestDto.AuthorId };
        var handler = new TestHttpMessageHandler(async (request, cancellationToken) =>
        {
            Assert.Equal(HttpMethod.Put, request.Method);
            Assert.Equal("http://localhost/api/v1/books/book-1", request.RequestUri!.ToString());
            var payload = await request.Content!.ReadFromJsonAsync<UpdateBookRequestDto>(cancellationToken);
            Assert.Equal(requestDto, payload);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expectedResponse)
            };
        });
        var booksClient = CreateBooksClient(handler);

        var book = await booksClient.Update("book-1", requestDto);

        Assert.Equal(expectedResponse.Id, book.Id);
        Assert.Equal(expectedResponse.Title, book.Title);
        Assert.Equal(expectedResponse.AuthorId, book.AuthorId);
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
