using System.Net.Http.Json;

using LibraryManagement.Api.Rest.Client.Domain.Authors.Create;

namespace LibraryManagement.Api.Rest.Client.Domain.Authors;

internal class AuthorsClient(IRestAPiClient client) : IAuthorsClient
{
    private const string BasePath = "api/v1/authors";
    private readonly HttpClient _httpClient = client.HttpClient;

    public async Task<AuthorDto> Create(CreateAuthorRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(BasePath, requestDto, cancellationToken);

        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<AuthorDto>(cancellationToken))!;
    }
}
