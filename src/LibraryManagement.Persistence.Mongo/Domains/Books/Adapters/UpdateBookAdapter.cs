using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Update;

using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.Domains.Books.Adapters;

public class UpdateBookAdapter(IBookCollection bookCollection, IBookEntityMapper mapper) : IUpdateBookPort
{
    public async Task<Book> Update(string id, string title, string authorId)
    {
        var updatedEntity = await bookCollection.Collection.FindOneAndUpdateAsync(
            entity => entity.Id == id,
            Builders<BookEntity>.Update
                .Set(entity => entity.Title, title)
                .Set(entity => entity.AuthorId, authorId),
            new FindOneAndUpdateOptions<BookEntity>
            {
                ReturnDocument = ReturnDocument.After
            });

        if (updatedEntity is null)
        {
            throw new InvalidOperationException($"Book '{id}' was not found.");
        }

        return mapper.ToDomain(updatedEntity);
    }
}
