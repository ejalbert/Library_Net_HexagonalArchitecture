using LibraryManagement.Domain.Domains.Books;
using Riok.Mapperly.Abstractions;

namespace LibraryManagement.Persistence.Mongo.Domains.Books;

public interface IBookEntityMapper
{
    Book ToDomain(BookEntity bookEntity);
    BookEntity ToEntity(Book book);
}
    

[Mapper]
public partial class BookEntityMapper : IBookEntityMapper
{
    public partial Book ToDomain(BookEntity bookEntity);
    public partial BookEntity ToEntity(Book book);
}