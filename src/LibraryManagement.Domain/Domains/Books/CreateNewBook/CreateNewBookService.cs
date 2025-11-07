using Microsoft.Extensions.Logging;

namespace LibraryManagement.Domain.Domains.Books.CreateNewBook;

internal class CreateNewBookService(ICreateNewBookPort createNewBookPort, ILogger<CreateNewBookService> logger):ICreateNewBookUseCase
{
    public Task<Book> Create(CreateNewBookCommand command)
    {
        return createNewBookPort.Create(command.Title);
    }
}