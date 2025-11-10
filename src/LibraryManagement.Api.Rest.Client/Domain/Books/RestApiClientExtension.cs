namespace LibraryManagement.Api.Rest.Client.Domain.Books;

public static class RestApiClientExtension
{
    extension(IRestAPiClient source)
    {
        public IBooksClient Books => new BooksClient(source);
    }
}