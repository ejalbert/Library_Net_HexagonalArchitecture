using System.Diagnostics;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LibraryManagement.AppHost;

internal static class OpenApiUiExstensions
{
    extension<T>(IResourceBuilder<T> builder) where T : IResourceWithEndpoints
    {
        internal IResourceBuilder<T> WithSwagger(string name = "swagger-ui-docs", string displayName = "Swagger API Documentation", string path = "/swagger")
        {
            return builder.WithOpenApiUi(name, displayName, path,
                "Document", IconVariant.Filled);
        }

        internal IResourceBuilder<T> WithRedoc(string name = "redoc-docs", string displayName = "ReDoc API Documentation", string path = "/api-docs")
        {
            return builder.WithOpenApiUi(name, displayName, path,
                "Document", IconVariant.Filled);
        }

        internal IResourceBuilder<T> WithScalar(string name = "scalar-docs", string displayName = "Scalar API Documentation", string path = "/scalar/v1")
        {
            return builder.WithOpenApiUi(name, displayName, path,
                "Document", IconVariant.Filled);
        }

        private IResourceBuilder<T> WithOpenApiUi(string name,
            string displayName, string path, string iconName, IconVariant iconVariant)
        {
            return builder.WithCommand(name, displayName, executeCommand: _ =>
            {
                try
                {
                    var endpoint = builder.GetEndpoint("https");

                    var url =  $"{endpoint.Url}{path}";

                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true});

                    return Task.FromResult(new ExecuteCommandResult
                    {
                        Success = true
                    });
                }
                catch (Exception ex)
                {
                    return Task.FromResult(new ExecuteCommandResult {Success = false, ErrorMessage = ex.ToString()});
                }


            }, new CommandOptions()
            {
                UpdateState = context=>context.ResourceSnapshot.HealthStatus == HealthStatus.Healthy ? ResourceCommandState.Enabled : ResourceCommandState.Disabled,
                IconName = iconName,
                IconVariant = iconVariant
            });
        }
    }
}
