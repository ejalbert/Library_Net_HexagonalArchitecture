namespace LibraryManagement.Domain.Domains.Books.GetSingleBook;

internal class GetSingleBookService(IGetSingleBookPort getSingleBookPort) : IGetSingleBookUseCase
{
    public Task<Book> Get(GetSingleBookCommand command)
    {
        return getSingleBookPort.GetById(command.Id);
    }
}