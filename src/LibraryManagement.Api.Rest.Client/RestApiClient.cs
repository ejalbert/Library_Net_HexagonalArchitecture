namespace LibraryManagement.Api.Rest.Client;

public class RestApiClient(HttpClient httpClient) : IRestAPiClient
{
    public HttpClient HttpClient => httpClient;
}