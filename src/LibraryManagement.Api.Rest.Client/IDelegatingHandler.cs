namespace LibraryManagement.Api.Rest.Client;

public interface IDelegatingHandler
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> next);
}
