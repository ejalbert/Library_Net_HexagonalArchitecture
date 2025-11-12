using System;
using LibraryManagement.Domain.Domains.Books;

namespace LibraryManagement.Domain.Domains.Books.Patch;

internal class PatchBookService(IPatchBookPort patchBookPort) : IPatchBookUseCase
{
    public Task<Book> Patch(PatchBookCommand command)
    {
        if (command.Title is null && command.AuthorId is null && command.Description is null && command.Keywords is null)
        {
            throw new ArgumentException("At least one field must be provided when patching a book.", nameof(command));
        }

        return patchBookPort.Patch(command.Id, command.Title, command.AuthorId, command.Description, command.Keywords);
    }
}
