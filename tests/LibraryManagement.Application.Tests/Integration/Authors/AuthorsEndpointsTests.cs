using System.Net.Http.Json;

using LibraryManagement.Api.Rest.Client.Domain.Authors;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Create;
using LibraryManagement.Application.Tests.Infrastructure;

namespace LibraryManagement.Application.Tests.Integration.Authors;

[Collection(ApplicationTestCollection.Name)]
public class AuthorsEndpointsTests
{
    private readonly ApplicationWebApplicationFactory _factory;

    public AuthorsEndpointsTests(ApplicationWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateAuthor_returns_created_author()
    {
        using HttpClient client = _factory.CreateClient();

        CreateAuthorRequestDto request = new("Refactoring Guru");

        using HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/authors", request);

        response.EnsureSuccessStatusCode();
        AuthorDto? dto = await response.Content.ReadFromJsonAsync<AuthorDto>();

        Assert.NotNull(dto);
        Assert.False(string.IsNullOrWhiteSpace(dto!.Id));
        Assert.Equal(request.Name, dto.Name);
    }
}
