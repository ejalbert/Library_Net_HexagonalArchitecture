using System.ComponentModel;
using System.Net.Http.Json;

using LibraryManagement.Api.Rest.Client.Domain.Books.Create;
using LibraryManagement.Api.Rest.Client.Domain.Books.Patch;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;
using LibraryManagement.Api.Rest.Client.Domain.Books.Update;

namespace LibraryManagement.Api.Rest.Client.Domain.Books;

internal class BooksClient(IRestAPiClient client) : IBooksClient
{
    private const string BasePath = "v1/books";
    private readonly HttpClient _httpClient = client.HttpClient;

    public async Task<BookDto> Create(CreateNewBookRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response =
            await client.HttpClient.PostAsJsonAsync($"{BasePath}", requestDto, cancellationToken);

        return (await response.Content.ReadFromJsonAsync<BookDto>(cancellationToken))!;
    }

    public Task<BookDto> Get([Description("Book identifier")] string bookId,
        CancellationToken cancellationToken = default)
    {
        return client.HttpClient.GetFromJsonAsync<BookDto>($"{BasePath}/{bookId}", cancellationToken)!;
    }

    public async Task<BookDto> Update([Description("Book identifier")] string bookId, UpdateBookRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response =
            await _httpClient.PutAsJsonAsync($"{BasePath}/{bookId}", requestDto, cancellationToken);

        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<BookDto>(cancellationToken))!;
    }

    public async Task<BookDto> Patch([Description("Book identifier")] string bookId, PatchBookRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        using HttpRequestMessage request = new(HttpMethod.Patch, $"{BasePath}/{bookId}")
        {
            Content = JsonContent.Create(requestDto)
        };

        using HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<BookDto>(cancellationToken))!;
    }

    public async Task<SearchBooksResponseDto> Search(SearchBooksRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response =
            await _httpClient.PostAsJsonAsync($"{BasePath}/search", requestDto, cancellationToken);

        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<SearchBooksResponseDto>(cancellationToken))!;
    }

    public async Task Delete([Description("Book identifier")] string bookId,
        CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"{BasePath}/{bookId}", cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}
