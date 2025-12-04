namespace LibraryManagement.Api.Rest.Client.Domain.Authors;

public static class RestApiClientAuthorExtension
{
    extension(IRestAPiClient source)
    {
        public IAuthorsClient Authors => new AuthorsClient(source);
    }
}
