using System.Linq.Expressions;

using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Domain.Domains.Authors.Search;
using LibraryManagement.Persistence.Postgres.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Domains.Authors.Adapters;

public class SearchAuthorsAdapter(LibraryManagementDbContext context, IAuthorEntityMapper mapper) : ISearchAuthorsPort
{
    public async Task<SearchResult<Author>> Search(string? searchTerm, Pagination pagination)
    {
        var term = searchTerm ?? string.Empty;
        Expression<Func<AuthorEntity, bool>> filter = author => EF.Functions.ILike(author.Name, $"%{term}%");

        var count = await context.Authors.LongCountAsync(filter);

        List<AuthorEntity> authors = await context.Authors
            .Where(filter)
            .Skip(pagination.PageIndex * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new SearchResult<Author>
        {
            Results = authors.Select(mapper.ToDomain),
            Pagination = new PaginationInfo
            {
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize,
                TotalItems = count
            }
        };
    }
}
