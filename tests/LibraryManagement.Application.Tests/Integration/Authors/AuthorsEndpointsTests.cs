using System.Net.Http.Json;

using LibraryManagement.Api.Rest.Client.Domain.Authors;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Create;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Search;
using LibraryManagement.Application.Tests.Infrastructure;
using LibraryManagement.Application.Tests.TestDoubles;
using LibraryManagement.Domain.Domains.Authors;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Application.Tests.Integration.Authors;

[Collection(ApplicationTestCollection.Name)]
public class AuthorsEndpointsTests
{
    private readonly ApplicationWebApplicationFactory _factory;
    private readonly InMemoryAuthorPersistence _persistence;

    public AuthorsEndpointsTests(ApplicationWebApplicationFactory factory)
    {
        _factory = factory;
        _persistence = factory.Services.GetRequiredService<InMemoryAuthorPersistence>();
        _persistence.Reset();
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

    [Fact]
    public async Task SearchAuthors_filters_results()
    {
        _persistence.Seed(
            new Author { Id = "author-1", Name = "Robert Martin" },
            new Author { Id = "author-2", Name = "Martin Fowler" },
            new Author { Id = "author-3", Name = "Kent Beck" });

        using HttpClient client = _factory.CreateClient();

        var searchRequest = new SearchAuthorsRequestDto("Martin");
        using HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/authors/search", searchRequest);

        response.EnsureSuccessStatusCode();
        SearchAuthorsResponseDto? payload = await response.Content.ReadFromJsonAsync<SearchAuthorsResponseDto>();

        Assert.NotNull(payload);
        Assert.Equal(2, payload!.Results.Count());
        Assert.Contains(payload.Results, dto => dto.Id == "author-1" && dto.Name == "Robert Martin");
        Assert.Contains(payload.Results, dto => dto.Id == "author-2" && dto.Name == "Martin Fowler");
    }
}
