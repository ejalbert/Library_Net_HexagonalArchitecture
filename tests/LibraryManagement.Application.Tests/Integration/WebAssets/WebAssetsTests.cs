using LibraryManagement.Application.Tests.Infrastructure;

namespace LibraryManagement.Application.Tests.Integration.WebAssets;

[Collection(ApplicationTestCollection.Name)]
public class WebAssetsTests
{
    private readonly ApplicationWebApplicationFactory _factory;

    public WebAssetsTests(ApplicationWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Root_path_serves_blazor_shell()
    {
        using HttpClient client = _factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync("/");

        response.EnsureSuccessStatusCode();
        string html = await response.Content.ReadAsStringAsync();

        Assert.Contains("<html", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("blazor.web.js", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Static_assets_are_exposed()
    {
        using HttpClient client = _factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync("/app.css");

        response.EnsureSuccessStatusCode();
        Assert.Equal("text/css", response.Content.Headers.ContentType?.MediaType);

        string css = await response.Content.ReadAsStringAsync();
        Assert.Contains(".content", css, StringComparison.OrdinalIgnoreCase);
    }
}
