using System.Net;
using System.Net.Http.Json;

using LibraryManagement.Api.Rest.Client.Common.Searches;
using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Api.Rest.Client.Domain.Books.Create;
using LibraryManagement.Api.Rest.Client.Domain.Books.Patch;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;
using LibraryManagement.Api.Rest.Client.Domain.Books.Update;
using LibraryManagement.Api.Rest.Client.Tests.Http;

namespace LibraryManagement.Api.Rest.Client.Tests.Domain.Books;

public class BooksClientTests
{
    [Fact]
    public async Task Create_SendsPostToBooksEndpointAndReturnsCreatedBook()
    {
        var requestDto =
            new CreateNewBookRequestDto("The Hobbit", "author-1", "A journey", new[] { "fantasy", "adventure" });
        var expectedResponse = new BookDto
        {
            Id = "book-1",
            Title = requestDto.Title,
            AuthorId = requestDto.AuthorId,
            Description = requestDto.Description,
            Keywords = requestDto.Keywords.ToArray()
        };
        var handler = new TestHttpMessageHandler(async (request, cancellationToken) =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("http://localhost/api/v1/books", request.RequestUri!.ToString());
            CreateNewBookRequestDto? payload =
                await request.Content!.ReadFromJsonAsync<CreateNewBookRequestDto>(cancellationToken);
            Assert.Equivalent(requestDto, payload);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expectedResponse)
            };
        });
        IBooksClient booksClient = CreateBooksClient(handler);

        BookDto book = await booksClient.Create(requestDto);

        Assert.Equal(expectedResponse.Id, book.Id);
        Assert.Equal(expectedResponse.Title, book.Title);
        Assert.Equal(expectedResponse.AuthorId, book.AuthorId);
        Assert.Equal(expectedResponse.Description, book.Description);
        Assert.Equal(expectedResponse.Keywords, book.Keywords);
    }

    [Fact]
    public async Task Get_SendsGetRequestAndReturnsBook()
    {
        var expectedResponse = new BookDto
        {
            Id = "book-1",
            Title = "The Hobbit",
            AuthorId = "author-1",
            Description = "A journey",
            Keywords = new[] { "fantasy", "adventure" }
        };
        var handler = new TestHttpMessageHandler((request, _) =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal("http://localhost/api/v1/books/book-1", request.RequestUri!.ToString());

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expectedResponse)
            });
        });
        IBooksClient booksClient = CreateBooksClient(handler);

        BookDto book = await booksClient.Get("book-1");

        Assert.Equal(expectedResponse.Id, book.Id);
        Assert.Equal(expectedResponse.Title, book.Title);
        Assert.Equal(expectedResponse.AuthorId, book.AuthorId);
        Assert.Equal(expectedResponse.Description, book.Description);
        Assert.Equal(expectedResponse.Keywords, book.Keywords);
    }

    [Fact]
    public async Task Search_SendsPostAndReturnsResponse()
    {
        var requestDto = new SearchBooksRequestDto("hobbit");
        var expectedResponse = new SearchBooksResponseDto([
            new BookDto
            {
                Id = "book-1",
                Title = "The Hobbit",
                AuthorId = "author-1",
                Description = "A journey",
                Keywords = new[] { "fantasy" }
            }
        ], new PaginationInfoDto(0, 1, 1));
        var handler = new TestHttpMessageHandler(async (request, cancellationToken) =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("http://localhost/api/v1/books/search", request.RequestUri!.ToString());
            SearchBooksRequestDto? payload =
                await request.Content!.ReadFromJsonAsync<SearchBooksRequestDto>(cancellationToken);
            Assert.Equal(requestDto, payload);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expectedResponse)
            };
        });
        IBooksClient booksClient = CreateBooksClient(handler);

        SearchBooksResponseDto response = await booksClient.Search(requestDto);

        BookDto dto = Assert.Single(response.Results);
        Assert.Equal("book-1", dto.Id);
        Assert.Equal("The Hobbit", dto.Title);
        Assert.Equal("author-1", dto.AuthorId);
        Assert.Equal("A journey", dto.Description);
        Assert.Equal(new[] { "fantasy" }, dto.Keywords);
    }

    [Fact]
    public async Task Search_WhenServerReturnsError_ThrowsHttpRequestException()
    {
        var handler = new TestHttpMessageHandler((_, _) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)));
        IBooksClient booksClient = CreateBooksClient(handler);

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
        IBooksClient booksClient = CreateBooksClient(handler);

        await booksClient.Delete("book-2");
    }

    [Fact]
    public async Task Update_SendsPutRequestAndReturnsBook()
    {
        var requestDto = new UpdateBookRequestDto("The Hobbit - Revised", "author-2", "Updated journey",
            new[] { "fantasy", "revised" });
        var expectedResponse = new BookDto
        {
            Id = "book-1",
            Title = requestDto.Title,
            AuthorId = requestDto.AuthorId,
            Description = requestDto.Description,
            Keywords = requestDto.Keywords.ToArray()
        };
        var handler = new TestHttpMessageHandler(async (request, cancellationToken) =>
        {
            Assert.Equal(HttpMethod.Put, request.Method);
            Assert.Equal("http://localhost/api/v1/books/book-1", request.RequestUri!.ToString());
            UpdateBookRequestDto? payload =
                await request.Content!.ReadFromJsonAsync<UpdateBookRequestDto>(cancellationToken);
            Assert.Equivalent(requestDto, payload);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expectedResponse)
            };
        });
        IBooksClient booksClient = CreateBooksClient(handler);

        BookDto book = await booksClient.Update("book-1", requestDto);

        Assert.Equal(expectedResponse.Id, book.Id);
        Assert.Equal(expectedResponse.Title, book.Title);
        Assert.Equal(expectedResponse.AuthorId, book.AuthorId);
        Assert.Equal(expectedResponse.Description, book.Description);
        Assert.Equal(expectedResponse.Keywords, book.Keywords);
    }

    [Fact]
    public async Task Patch_SendsPatchRequestAndReturnsBook()
    {
        var requestDto = new PatchBookRequestDto(Description: "New blurb", Keywords: new[] { "fantasy" });
        var expectedResponse = new BookDto
        {
            Id = "book-1",
            Title = "The Hobbit",
            AuthorId = "author-1",
            Description = "New blurb",
            Keywords = new[] { "fantasy" }
        };

        var handler = new TestHttpMessageHandler(async (request, cancellationToken) =>
        {
            Assert.Equal(HttpMethod.Patch, request.Method);
            Assert.Equal("http://localhost/api/v1/books/book-1", request.RequestUri!.ToString());
            PatchBookRequestDto? payload =
                await request.Content!.ReadFromJsonAsync<PatchBookRequestDto>(cancellationToken);
            Assert.Equivalent(requestDto, payload);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expectedResponse)
            };
        });
        IBooksClient booksClient = CreateBooksClient(handler);

        BookDto book = await booksClient.Patch("book-1", requestDto);

        Assert.Equal(expectedResponse.Id, book.Id);
        Assert.Equal(expectedResponse.Description, book.Description);
        Assert.Equal(expectedResponse.Keywords, book.Keywords);
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
