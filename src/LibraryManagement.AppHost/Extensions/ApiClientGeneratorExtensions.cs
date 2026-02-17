using System.Diagnostics;

namespace LibraryManagement.AppHost.Extensions;

internal static class ApiClientGeneratorExtensions
{
    private const string GeneratorProjectPath = "src/LibraryManagement.Api.Rest.Client.Generator";
    private const string ReactClientPath = "src/LibraryManagement.Web.React";
    private const string LocalLibrariesPath = "local-libraries";

    internal static IResourceBuilder<T> WithGenerateApiClientCommand<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<IResourceWithEndpoints> apiResource)
        where T : IResource
    {
        return builder.WithCommand(
            "generate-api-client",
            "Generate API Client",
            async context =>
            {
                var repoRoot = GetRepoRoot();
                var generatorPath = Path.Combine(repoRoot, GeneratorProjectPath);
                var reactClientPath = Path.Combine(repoRoot, ReactClientPath);
                var localLibrariesPath = Path.Combine(repoRoot, LocalLibrariesPath);

                // Get the API URL from the resource
                EndpointReference endpoint = apiResource.GetEndpoint("https");
                var apiUrl = $"{endpoint.Url}/api/v1.json";

                try
                {
                    // Ensure local-libraries folder exists
                    Directory.CreateDirectory(localLibrariesPath);

                    // Step 1: Retrieve OpenAPI spec (with injected URL)
                    ExecuteCommandResult retrieveResult = await RunNpmCommand(
                        generatorPath,
                        "run retreive-openapi-spec",
                        context.CancellationToken,
                        new Dictionary<string, string> { ["OPENAPI_URL"] = apiUrl });
                    if (!retrieveResult.Success)
                        return retrieveResult;

                    // Step 2: Generate TypeScript and C# clients
                    ExecuteCommandResult codegenResult =
                        await RunNpmCommand(generatorPath, "run codegen", context.CancellationToken);
                    if (!codegenResult.Success)
                        return codegenResult;

                    // Step 3: Build the TypeScript package
                    ExecuteCommandResult buildResult =
                        await RunNpmCommand(generatorPath, "run build:all", context.CancellationToken);
                    if (!buildResult.Success)
                        return buildResult;

                    // Step 4: Pack the TypeScript package
                    ExecuteCommandResult packResult =
                        await RunNpmCommand(generatorPath, "pack", context.CancellationToken);
                    if (!packResult.Success)
                        return packResult;

                    // Step 5: Build and pack the C# NuGet package
                    ExecuteCommandResult packCsharpResult =
                        await RunNpmCommand(generatorPath, "run pack:csharp", context.CancellationToken);
                    if (!packCsharpResult.Success)
                        return packCsharpResult;

                    // Step 6: Copy TypeScript .tgz to local-libraries
                    var tgzFiles = Directory.GetFiles(generatorPath, "*.tgz");
                    if (tgzFiles.Length == 0)
                        return new ExecuteCommandResult
                        {
                            Success = false,
                            ErrorMessage = "No .tgz package file found after npm pack"
                        };

                    var tgzFile = tgzFiles.OrderByDescending(File.GetLastWriteTime).First();
                    var tgzFileName = Path.GetFileName(tgzFile);
                    var destinationTgz = Path.Combine(localLibrariesPath, tgzFileName);
                    File.Copy(tgzFile, destinationTgz, true);
                    File.Delete(tgzFile); // Clean up from generator folder

                    // Step 7: Copy C# .nupkg to local-libraries
                    var nupkgPath = Path.Combine(generatorPath, "nupkg");
                    if (Directory.Exists(nupkgPath))
                    {
                        var nupkgFiles = Directory.GetFiles(nupkgPath, "*.nupkg");
                        foreach (var nupkgFile in nupkgFiles)
                        {
                            var nupkgFileName = Path.GetFileName(nupkgFile);
                            var destinationNupkg = Path.Combine(localLibrariesPath, nupkgFileName);
                            File.Copy(nupkgFile, destinationNupkg, true);
                        }
                    }

                    // Step 8: Install the TypeScript package in React client from local-libraries
                    var relativeTgzPath = Path.GetRelativePath(reactClientPath, destinationTgz);
                    ExecuteCommandResult installResult = await RunNpmCommand(reactClientPath,
                        $"install {relativeTgzPath}", context.CancellationToken);
                    if (!installResult.Success)
                        return installResult;


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
            foreach (var (key, value) in environmentVariables)
                startInfo.Environment[key] = value;

        try
        {
            using var process = Process.Start(startInfo);
            if (process == null)
                return new ExecuteCommandResult
                {
                    Success = false,
                    ErrorMessage = "Failed to start npm process"
                };

            await process.WaitForExitAsync(cancellationToken);

            if (process.ExitCode != 0)
            {
                var error = await process.StandardError.ReadToEndAsync(cancellationToken);
                var output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
                return new ExecuteCommandResult
                {
                    Success = false,
                    ErrorMessage =
                        $"npm {arguments} failed with exit code {process.ExitCode}\nOutput: {output}\nError: {error}"
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
            directory = directory.Parent;

        return directory?.FullName ?? Directory.GetCurrentDirectory();
    }
}
