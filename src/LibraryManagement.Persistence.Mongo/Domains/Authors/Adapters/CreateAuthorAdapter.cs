using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Domain.Domains.Authors.Create;

namespace LibraryManagement.Persistence.Mongo.Domains.Authors.Adapters;

public class CreateAuthorAdapter(IAuthorCollection authorCollection, IAuthorEntityMapper authorEntityMapper)
    : ICreateAuthorPort
{
    public async Task<Author> Create(string name)
    {
        AuthorEntity authorEntity = new() { Name = name };

        await authorCollection.Collection.InsertOneAsync(authorEntity);

        return authorEntityMapper.ToDomain(authorEntity);
    }
}
