using System.Net.Http.Json;

using LibraryManagement.Api.Rest.Client.Domain.Books.Create;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;

namespace LibraryManagement.Api.Rest.Client.Domain.Books;

internal class BooksClient(IRestAPiClient client) : IBooksClient
{
    private const string BasePath = "api/v1/books";
    private readonly HttpClient _httpClient = client.HttpClient;

    public async Task<BookDto> Create(CreateNewBookRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        var response = await client.HttpClient.PostAsJsonAsync($"{BasePath}", requestDto, cancellationToken);

        return (await response.Content.ReadFromJsonAsync<BookDto>(cancellationToken: cancellationToken))!;
    }

    public Task<BookDto> Get(string bookId, CancellationToken cancellationToken = default)
    {
        return client.HttpClient.GetFromJsonAsync<BookDto>($"{BasePath}/{bookId}", cancellationToken)!;
    }

    public async Task<SearchBooksResponseDto> Search(SearchBooksRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"{BasePath}/search", requestDto, cancellationToken);

        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<SearchBooksResponseDto>(cancellationToken))!;
    }
}
