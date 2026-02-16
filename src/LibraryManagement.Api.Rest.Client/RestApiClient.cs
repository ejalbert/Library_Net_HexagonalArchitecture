namespace LibraryManagement.Api.Rest.Client;

public class RestApiClient : IRestAPiClient
{
    public RestApiClient(HttpClient httpClient)
    {
        HttpClient = EnsureApiBasePath(httpClient);
    }

    public HttpClient HttpClient { get; }

    private static HttpClient EnsureApiBasePath(HttpClient httpClient)
    {
        Uri? baseAddress = httpClient.BaseAddress;
        if (baseAddress == null) return httpClient;

        var basePath = baseAddress.ToString().TrimEnd('/');

        if (!basePath.EndsWith("/api", StringComparison.OrdinalIgnoreCase)) basePath += "/api";

        httpClient.BaseAddress = new Uri($"{basePath}/");
        return httpClient;
    }
}
