using System.Net;
using System.Net.Http.Json;

using LibraryManagement.Api.Rest.Client.Common.Searches;
using LibraryManagement.Api.Rest.Client.Domain.Authors;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Create;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Search;
using LibraryManagement.Api.Rest.Client.Tests.Http;

namespace LibraryManagement.Api.Rest.Client.Tests.Domain.Authors;

public class AuthorsClientTests
{
    [Fact]
    public async Task Create_SendsPostAndReturnsAuthor()
    {
        CreateAuthorRequestDto request = new("Kent Beck");
        AuthorDto expected = new() { Id = "author-1", Name = "Kent Beck" };

        TestHttpMessageHandler handler = new(async (message, token) =>
        {
            Assert.Equal(HttpMethod.Post, message.Method);
            Assert.Equal("http://localhost/api/v1/authors", message.RequestUri!.ToString());
            CreateAuthorRequestDto? payload = await message.Content!.ReadFromJsonAsync<CreateAuthorRequestDto>(token);
            Assert.Equal(request, payload);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expected)
            };
        });

        AuthorsClient client = CreateClient(handler);

        AuthorDto author = await client.Create(request);

        Assert.Equal(expected.Id, author.Id);
        Assert.Equal(expected.Name, author.Name);
    }

    [Fact]
    public async Task Search_SendsPostAndReturnsAuthors()
    {
        SearchAuthorsRequestDto request = new("Martin");
        SearchAuthorsResponseDto expected =
            new([new AuthorDto { Id = "author-1", Name = "George Martin" }], new PaginationInfoDto(0, 1, 1));

        TestHttpMessageHandler handler = new(async (message, token) =>
        {
            Assert.Equal(HttpMethod.Post, message.Method);
            Assert.Equal("http://localhost/api/v1/authors/search", message.RequestUri!.ToString());
            SearchAuthorsRequestDto? payload = await message.Content!.ReadFromJsonAsync<SearchAuthorsRequestDto>(token);
            Assert.Equal(request, payload);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expected)
            };
        });

        AuthorsClient client = CreateClient(handler);

        SearchAuthorsResponseDto response = await client.Search(request);

        AuthorDto author = Assert.Single(response.Results);
        Assert.Equal("author-1", author.Id);
        Assert.Equal("George Martin", author.Name);
    }

    [Fact]
    public async Task Search_WhenServerReturnsError_ThrowsHttpRequestException()
    {
        TestHttpMessageHandler handler = new((_, _) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)));
        AuthorsClient client = CreateClient(handler);

        await Assert.ThrowsAsync<HttpRequestException>(() => client.Search(new SearchAuthorsRequestDto("martin")));
    }

    private static AuthorsClient CreateClient(HttpMessageHandler handler)
    {
        HttpClient httpClient = new(handler)
        {
            BaseAddress = new Uri("http://localhost/api")
        };

        return new AuthorsClient(new RestApiClient(httpClient));
    }
}
