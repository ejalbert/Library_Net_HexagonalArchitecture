using System.Net;
using System.Net.Http.Json;

using LibraryManagement.Api.Rest.Client.Domain.Authors;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Create;
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

    private static AuthorsClient CreateClient(HttpMessageHandler handler)
    {
        HttpClient httpClient = new(handler)
        {
            BaseAddress = new Uri("http://localhost/api")
        };

        return new AuthorsClient(new RestApiClient(httpClient));
    }
}
