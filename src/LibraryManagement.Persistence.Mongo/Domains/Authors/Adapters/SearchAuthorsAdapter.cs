using System.Linq.Expressions;

using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Domain.Domains.Authors.Search;

using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.Domains.Authors.Adapters;

public class SearchAuthorsAdapter(IAuthorCollection authorCollection, IAuthorEntityMapper mapper) : ISearchAuthorsPort
{
    public async Task<SearchResult<Author>> Search(string? searchTerm, Pagination pagination)
    {
        Expression<Func<AuthorEntity, bool>> filter = author =>
            searchTerm == null ||
            author.Name.Contains(searchTerm);

        var count = await authorCollection.Collection.CountDocumentsAsync(filter);

        IAsyncCursor<AuthorEntity> searchRequest = await authorCollection.Collection.FindAsync(
            filter,
            new FindOptions<AuthorEntity>
            {
                Skip = pagination.PageIndex * pagination.PageSize,
                Limit = pagination.PageSize
            });

        return new SearchResult<Author>
        {
            Results = searchRequest.ToEnumerable().Select(mapper.ToDomain).ToList(),
            Pagination = new PaginationInfo
            {
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize,
                TotalItems = count
            }
        };
    }
}
