using Microsoft.Extensions.Logging;

namespace LibraryManagement.Domain.Domains.Books.Delete;

internal class DeleteBookService(IDeleteBookPort deleteBookPort, ILogger<DeleteBookService> logger) : IDeleteBookUseCase
{
    public Task Delete(DeleteBookCommand command)
    {
        return deleteBookPort.Delete(command.Id);
    }
}
