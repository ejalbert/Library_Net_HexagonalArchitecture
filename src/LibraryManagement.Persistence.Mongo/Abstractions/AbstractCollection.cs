using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.Abstractions;

public abstract class AbstractCollection<TEntity>(IMongoCollection<TEntity> collection) : IAbstractCollection<TEntity>
{
    public IMongoCollection<TEntity> Collection { get; } = collection;
}
