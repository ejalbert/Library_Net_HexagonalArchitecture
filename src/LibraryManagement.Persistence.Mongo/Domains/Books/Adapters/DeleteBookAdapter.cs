using LibraryManagement.Domain.Domains.Books.Delete;

using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.Domains.Books.Adapters;

public class DeleteBookAdapter(IBookCollection bookCollection) : IDeleteBookPort
{
    public async Task Delete(string id)
    {
        DeleteResult result = await bookCollection.Collection.DeleteOneAsync(entity => entity.Id == id);

        if (result.DeletedCount == 0) throw new InvalidOperationException($"Book '{id}' was not found.");
    }
}
