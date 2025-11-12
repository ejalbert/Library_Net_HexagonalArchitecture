using LibraryManagement.Domain.Domains.Books;

using Microsoft.Extensions.Logging;

namespace LibraryManagement.Domain.Domains.Books.Update;

internal class UpdateBookService(IUpdateBookPort updateBookPort, ILogger<UpdateBookService> logger) : IUpdateBookUseCase
{
    public Task<Book> Update(UpdateBookCommand command)
    {
        return updateBookPort.Update(command.Id, command.Title, command.AuthorId, command.Description, command.Keywords);
    }
}
