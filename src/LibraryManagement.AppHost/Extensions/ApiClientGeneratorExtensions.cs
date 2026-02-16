using System.Diagnostics;

namespace LibraryManagement.AppHost.Extensions;

internal static class ApiClientGeneratorExtensions
{
    private const string GeneratorProjectPath = "src/LibraryManagement.Api.Rest.Client.Generator";
    private const string ReactClientPath = "src/LibraryManagement.Web.React";

    internal static IResourceBuilder<T> WithGenerateApiClientCommand<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<IResourceWithEndpoints> apiResource)
        where T : IResource
    {
        return builder.WithCommand(
            name: "generate-api-client",
            displayName: "Generate API Client",
            executeCommand: async context =>
            {
                var repoRoot = GetRepoRoot();
                var generatorPath = Path.Combine(repoRoot, GeneratorProjectPath);
                var reactClientPath = Path.Combine(repoRoot, ReactClientPath);

                // Get the API URL from the resource
                var endpoint = apiResource.GetEndpoint("https");
                var apiUrl = $"{endpoint.Url}/api/v1.json";

                try
                {
                    // Step 1: Retrieve OpenAPI spec (with injected URL)
                    var retrieveResult = await RunNpmCommand(
                        generatorPath,
                        "run retreive-openapi-spec",
                        context.CancellationToken,
                        new Dictionary<string, string> { ["OPENAPI_URL"] = apiUrl });
                    if (!retrieveResult.Success)
                        return retrieveResult;

                    // Step 2: Generate TypeScript client
                    var codegenResult = await RunNpmCommand(generatorPath, "run codegen", context.CancellationToken);
                    if (!codegenResult.Success)
                        return codegenResult;

                    // Step 3: Build the package
                    var buildResult = await RunNpmCommand(generatorPath, "run build", context.CancellationToken);
                    if (!buildResult.Success)
                        return buildResult;

                    // Step 4: Pack the package
                    var packResult = await RunNpmCommand(generatorPath, "pack", context.CancellationToken);
                    if (!packResult.Success)
                        return packResult;

                    // Step 5: Copy and install in React client
                    // Find the generated .tgz file
                    var tgzFiles = Directory.GetFiles(generatorPath, "*.tgz");
                    if (tgzFiles.Length == 0)
                    {
                        return new ExecuteCommandResult
                        {
                            Success = false,
                            ErrorMessage = "No .tgz package file found after npm pack"
                        };
                    }

                    var tgzFile = tgzFiles.OrderByDescending(File.GetLastWriteTime).First();
                    var tgzFileName = Path.GetFileName(tgzFile);
                    var destinationTgz = Path.Combine(reactClientPath, tgzFileName);

                    // Copy .tgz to React client directory
                    File.Copy(tgzFile, destinationTgz, overwrite: true);

                    // Install the package in React client
                    var installResult = await RunNpmCommand(reactClientPath, $"install ./{tgzFileName}", context.CancellationToken);
                    if (!installResult.Success)
                        return installResult;

                    // Clean up .tgz files
                    File.Delete(tgzFile);
                    //File.Delete(destinationTgz);

                    return new ExecuteCommandResult { Success = true };
                }
                catch (Exception ex)
                {
                    return new ExecuteCommandResult
                    {
                        Success = false,
                        ErrorMessage = ex.ToString()
                    };
                }
            },
            new CommandOptions
            {
                IconName = "ArrowSync",
                IconVariant = IconVariant.Filled
            }
        );
    }

    private static async Task<ExecuteCommandResult> RunNpmCommand(
        string workingDirectory,
        string arguments,
        CancellationToken cancellationToken,
        Dictionary<string, string>? environmentVariables = null)
    {
        var startInfo = new ProcessStartInfo("npm", arguments)
        {
            UseShellExecute = false,
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        if (environmentVariables != null)
        {
            foreach (var (key, value) in environmentVariables)
            {
                startInfo.Environment[key] = value;
            }
        }

        try
        {
            using var process = Process.Start(startInfo);
            if (process == null)
            {
                return new ExecuteCommandResult
                {
                    Success = false,
                    ErrorMessage = "Failed to start npm process"
                };
            }

            await process.WaitForExitAsync(cancellationToken);

            if (process.ExitCode != 0)
            {
                var error = await process.StandardError.ReadToEndAsync(cancellationToken);
                var output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
                return new ExecuteCommandResult
                {
                    Success = false,
                    ErrorMessage = $"npm {arguments} failed with exit code {process.ExitCode}\nOutput: {output}\nError: {error}"
                };
            }

            return new ExecuteCommandResult { Success = true };
        }
        catch (Exception ex)
        {
            return new ExecuteCommandResult
            {
                Success = false,
                ErrorMessage = $"Failed to run npm {arguments}: {ex.Message}"
            };
        }
    }

    private static string GetRepoRoot()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "LibraryManagement.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? Directory.GetCurrentDirectory();
    }
}



