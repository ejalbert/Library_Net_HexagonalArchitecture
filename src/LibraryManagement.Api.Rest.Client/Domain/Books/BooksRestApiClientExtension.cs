namespace LibraryManagement.Api.Rest.Client.Domain.Books;

public static class BooksRestApiClientExtension
{
    extension(IRestAPiClient source)
    {
        public IBooksClient Books => new BooksClient(source);
    }
}
