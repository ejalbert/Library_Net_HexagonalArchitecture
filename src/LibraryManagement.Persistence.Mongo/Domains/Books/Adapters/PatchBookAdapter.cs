using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Patch;

using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.Domains.Books.Adapters;

public class PatchBookAdapter(IBookCollection bookCollection, IBookEntityMapper mapper) : IPatchBookPort
{
    public async Task<Book> Patch(string id, string? title, string? authorId, string? description,
        IReadOnlyCollection<string>? keywords)
    {
        UpdateDefinition<BookEntity>? updateDefinition = BuildUpdateDefinition(title, authorId, description, keywords);
        BookEntity? updatedEntity;

        if (updateDefinition is not null)
        {
            updatedEntity = await bookCollection.Collection.FindOneAndUpdateAsync(
                entity => entity.Id == id,
                updateDefinition,
                new FindOneAndUpdateOptions<BookEntity>
                {
                    ReturnDocument = ReturnDocument.After
                });
        }
        else
        {
            IAsyncCursor<BookEntity>? cursor = await bookCollection.Collection.FindAsync(entity => entity.Id == id);
            updatedEntity = cursor.SingleOrDefault();
        }

        if (updatedEntity is null) throw new InvalidOperationException($"Book '{id}' was not found.");

        return mapper.ToDomain(updatedEntity);
    }

    private static UpdateDefinition<BookEntity>? BuildUpdateDefinition(string? title, string? authorId,
        string? description, IReadOnlyCollection<string>? keywords)
    {
        List<UpdateDefinition<BookEntity>> updates = new();
        if (title is not null) updates.Add(Builders<BookEntity>.Update.Set(entity => entity.Title, title));

        if (authorId is not null) updates.Add(Builders<BookEntity>.Update.Set(entity => entity.AuthorId, authorId));

        if (description is not null)
            updates.Add(Builders<BookEntity>.Update.Set(entity => entity.Description, description));

        if (keywords is not null)
            updates.Add(Builders<BookEntity>.Update.Set(entity => entity.Keywords, keywords.ToList()));

        if (updates.Count == 0) return null;

        return Builders<BookEntity>.Update.Combine(updates);
    }
}
