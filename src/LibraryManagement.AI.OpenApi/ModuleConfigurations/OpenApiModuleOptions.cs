namespace LibraryManagement.AI.OpenApi.ModuleConfigurations;

public class OpenApiModuleOptions
{
    public string ApiKey { get; set; }
    public string BaseUrl { get; set; } // e.g., https://api.openai.com/
    public string Model { get; set; } = "gpt-4o-mini";
}
