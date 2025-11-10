namespace LibraryManagement.Domain.Domains.Books.GetSingle;

internal class GetSingleBookService(IGetSingleBookPort getSingleBookPort) : IGetSingleBookUseCase
{
    public Task<Book> Get(GetSingleBookCommand command)
    {
        return getSingleBookPort.GetById(command.Id);
    }
}