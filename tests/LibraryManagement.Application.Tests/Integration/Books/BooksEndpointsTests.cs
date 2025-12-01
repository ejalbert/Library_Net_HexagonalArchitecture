using System.Net;
using System.Net.Http.Json;

using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Api.Rest.Client.Domain.Books.Create;
using LibraryManagement.Api.Rest.Client.Domain.Books.Patch;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;
using LibraryManagement.Application.Tests.Infrastructure;
using LibraryManagement.Application.Tests.TestDoubles;
using LibraryManagement.Domain.Domains.Books;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Application.Tests.Integration.Books;

[Collection(ApplicationTestCollection.Name)]
public class BooksEndpointsTests
{
    private readonly ApplicationWebApplicationFactory _factory;
    private readonly InMemoryBookPersistence _persistence;

    public BooksEndpointsTests(ApplicationWebApplicationFactory factory)
    {
        _factory = factory;
        _persistence = factory.Services.GetRequiredService<InMemoryBookPersistence>();
        _persistence.Reset();
    }

    [Fact]
    public async Task GetBook_returns_seeded_book()
    {
        Book seeded = new()
        {
            Id = "book-1",
            Title = "Hexagonal Architecture in Action",
            AuthorId = "author-1",
            Description = "Practical guide",
            Keywords = new[] { "architecture", "hexagonal" }
        };
        _persistence.Seed(seeded);

        using HttpClient client = _factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync("/api/v1/books/book-1");

        response.EnsureSuccessStatusCode();
        BookDto? dto = await response.Content.ReadFromJsonAsync<BookDto>();

        Assert.NotNull(dto);
        Assert.Equal(seeded.Id, dto!.Id);
        Assert.Equal(seeded.Title, dto.Title);
        Assert.Equal(seeded.AuthorId, dto.AuthorId);
        Assert.Equal(seeded.Description, dto.Description);
        Assert.Equal(seeded.Keywords, dto.Keywords);
    }

    [Fact]
    public async Task CreateBook_persists_data()
    {
        using HttpClient client = _factory.CreateClient();

        var request = new CreateNewBookRequestDto("Domain-Driven Design", "author-9", "DDD classic",
            new[] { "ddd", "architecture" });
        using HttpResponseMessage createResponse = await client.PostAsJsonAsync("/api/v1/books", request);

        createResponse.EnsureSuccessStatusCode();
        BookDto? created = await createResponse.Content.ReadFromJsonAsync<BookDto>();

        Assert.NotNull(created);
        Assert.False(string.IsNullOrWhiteSpace(created!.Id));

        BookDto? fetched = await client.GetFromJsonAsync<BookDto>($"/api/v1/books/{created.Id}");

        Assert.NotNull(fetched);
        Assert.Equal(request.Title, fetched!.Title);
        Assert.Equal(request.AuthorId, fetched.AuthorId);
        Assert.Equal(request.Description, fetched.Description);
        Assert.Equal(request.Keywords, fetched.Keywords);
    }

    [Fact]
    public async Task SearchBooks_filters_results()
    {
        _persistence.Seed(
            new Book
            {
                Id = "book-1",
                Title = "Pragmatic Hexagonal Architecture",
                AuthorId = "author-1",
                Description = "Pragmatic guide",
                Keywords = new[] { "architecture" }
            },
            new Book
            {
                Id = "book-2",
                Title = "CQRS Patterns",
                AuthorId = "author-2",
                Description = "CQRS deep dive",
                Keywords = new[] { "cqrs" }
            }
        );

        using HttpClient client = _factory.CreateClient();

        var searchRequest = new SearchBooksRequestDto("Hexagonal");
        using HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/books/search", searchRequest);

        response.EnsureSuccessStatusCode();
        SearchBooksResponseDto? payload = await response.Content.ReadFromJsonAsync<SearchBooksResponseDto>();

        Assert.NotNull(payload);
        BookDto book = Assert.Single(payload!.Results);
        Assert.Equal("book-1", book.Id);
        Assert.Equal("author-1", book.AuthorId);
        Assert.Equal("author-1", book.AuthorId);
    }

    [Fact]
    public async Task DeleteBook_removes_persisted_entry()
    {
        Book seeded = new()
        {
            Id = "book-9",
            Title = "Implementing Hexagonal Architecture",
            AuthorId = "author-3",
            Description = "Implementation details",
            Keywords = new[] { "architecture", "implementation" }
        };
        _persistence.Seed(seeded);

        using HttpClient client = _factory.CreateClient();

        using HttpResponseMessage response = await client.DeleteAsync($"/api/v1/books/{seeded.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _persistence.GetById(seeded.Id));
    }

    [Fact]
    public async Task PatchBook_updates_selected_fields()
    {
        Book seeded = new()
        {
            Id = "book-22",
            Title = "Event-Driven Architectures",
            AuthorId = "author-22",
            Description = "Original",
            Keywords = new[] { "eda" }
        };
        _persistence.Seed(seeded);

        using HttpClient client = _factory.CreateClient();

        PatchBookRequestDto request = new(Description: "Updated description", Keywords: new[] { "eda", "messaging" });
        using HttpResponseMessage response = await client.PatchAsJsonAsync($"/api/v1/books/{seeded.Id}", request);

        response.EnsureSuccessStatusCode();
        BookDto? patched = await response.Content.ReadFromJsonAsync<BookDto>();

        Assert.NotNull(patched);
        Assert.Equal(seeded.Id, patched!.Id);
        Assert.Equal(seeded.Title, patched.Title);
        Assert.Equal(seeded.AuthorId, patched.AuthorId);
        Assert.Equal("Updated description", patched.Description);
        Assert.Equal(new[] { "eda", "messaging" }, patched.Keywords);
    }
}
