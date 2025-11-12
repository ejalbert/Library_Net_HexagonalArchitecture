using LibraryManagement.Persistence.Mongo.Domains.Books;
using LibraryManagement.Persistence.Mongo.Domains.Books.Adapters;
using LibraryManagement.Persistence.Mongo.Tests.Fixtures;

using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.Tests.Domains.Books;

[Collection(nameof(MongoDbCollection))]
public sealed class BookAdaptersIntegrationTests(MongoDbContainerFixture fixture) : IAsyncLifetime
{
    private IMongoDatabase? _database;
    private IBookCollection? _bookCollection;
    private readonly BookEntityMapper _mapper = new();

    public Task InitializeAsync()
    {
        _database = fixture.CreateCleanDatabase();
        _bookCollection = new BookCollection(_database);
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        if (_database is null)
        {
            return;
        }

        await fixture.DropDatabaseAsync(_database);
    }

    [Fact]
    public async Task CreateNewBookAdapter_persists_and_returns_domain_entity()
    {
        CreateNewBookAdapter adapter = new(GetBookCollection(), _mapper);

        var result = await adapter.Create("Patterns of Enterprise Application Architecture", "author-1");

        BookEntity persisted = await GetBookCollection().Collection
            .Find(entity => entity.Id == result.Id)
            .SingleAsync();

        Assert.Equal("Patterns of Enterprise Application Architecture", persisted.Title);
        Assert.Equal("author-1", persisted.AuthorId);
        Assert.Equal(persisted.Id, result.Id);
        Assert.Equal(persisted.AuthorId, result.AuthorId);
    }

    [Fact]
    public async Task GetSingleBookAdapter_returns_stored_book()
    {
        BookEntity seeded = await SeedBook("Domain-Driven Design", "author-2");

        GetSingleBookAdapter adapter = new(GetBookCollection(), _mapper);

        var result = await adapter.GetById(seeded.Id);

        Assert.Equal(seeded.Id, result.Id);
        Assert.Equal("Domain-Driven Design", result.Title);
    }

    [Fact]
    public async Task UpdateBookAdapter_updates_existing_book_title()
    {
        BookEntity seeded = await SeedBook("Clean Code", "author-3");

        UpdateBookAdapter adapter = new(GetBookCollection(), _mapper);

        var result = await adapter.Update(seeded.Id, "Clean Code (2nd Edition)", "author-4");

        Assert.Equal(seeded.Id, result.Id);
        Assert.Equal("Clean Code (2nd Edition)", result.Title);
        Assert.Equal("author-4", result.AuthorId);

        BookEntity reloaded = await GetBookCollection().Collection
            .Find(entity => entity.Id == seeded.Id)
            .SingleAsync();

        Assert.Equal("Clean Code (2nd Edition)", reloaded.Title);
        Assert.Equal("author-4", reloaded.AuthorId);
    }

    [Fact]
    public async Task SearchBooksAdapter_filters_by_partial_title()
    {
        await SeedBook("Clean Code", "author-5");
        await SeedBook("Refactoring", "author-6");
        await SeedBook("The Pragmatic Programmer", "author-7");

        SearchBooksAdapter adapter = new(GetBookCollection(), _mapper);

        var results = await adapter.Search("ing");

        var titles = results.Select(book => book.Title).ToList();

        Assert.Contains("Refactoring", titles);
        Assert.DoesNotContain("Clean Code", titles);
        Assert.DoesNotContain("The Pragmatic Programmer", titles);
    }

    [Fact]
    public async Task DeleteBookAdapter_removes_existing_book()
    {
        BookEntity seeded = await SeedBook("Working Effectively with Legacy Code", "author-8");

        DeleteBookAdapter adapter = new(GetBookCollection());

        await adapter.Delete(seeded.Id);

        long remaining = await GetBookCollection().Collection
            .CountDocumentsAsync(entity => entity.Id == seeded.Id);

        Assert.Equal(0, remaining);
    }

    private IBookCollection GetBookCollection()
    {
        return _bookCollection ?? throw new InvalidOperationException("Book collection has not been initialized yet.");
    }

    private async Task<BookEntity> SeedBook(string title, string authorId)
    {
        BookEntity entity = new()
        {
            Title = title,
            AuthorId = authorId
        };

        await GetBookCollection().Collection.InsertOneAsync(entity);

        return entity;
    }
}
