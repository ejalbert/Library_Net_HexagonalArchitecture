using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.Abstractions;

public interface IAbstractCollection<TEntity>
{
    IMongoCollection<TEntity> Collection { get; }
}
