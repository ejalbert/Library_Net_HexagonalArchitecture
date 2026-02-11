using System.Diagnostics;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LibraryManagement.AppHost;

internal static class EfCoreExtensions
{
    private const string MigrationsProjectPath = "src/LibraryManagement.Persistence.Postgres.Migrations/LibraryManagement.Persistence.Postgres.Migrations.csproj";

    extension<T>(IResourceBuilder<T> builder) where T : IResourceWithConnectionString
    {
        internal IResourceBuilder<T> WithMigrateDatabaseCommand()
        {
            return builder.WithEfCoreCommands<T>(
                "migrate-database",
                "Migrate Database to Latest",
                _ =>
                {
                    return Task.FromResult("database update " +
                                           $"--project {MigrationsProjectPath} " +
                                           $"--startup-project {MigrationsProjectPath}");
                }
            );
        }

        internal IResourceBuilder<T> WithRevertAllMigrationsCommand()
        {
            return builder.WithEfCoreCommands<T>(
                "revert-all-database-migration",
                "Revert all Database Migration",
                _ =>
                {
                    return Task.FromResult("database update 0 " +
                                           $"--project {MigrationsProjectPath} " +
                                           $"--startup-project {MigrationsProjectPath}");
                }
            );
        }

        internal IResourceBuilder<T> WithRemoveMigrationCommand()
        {
            return builder.WithEfCoreCommands<T>(
                "remove-ef-migration",
                "Remove EF Migration",
                _ =>
                {
                    return Task.FromResult("migrations remove " +
                                           $"--project {MigrationsProjectPath} " +
                                           $"--startup-project {MigrationsProjectPath}");
                }
            );
        }

        internal IResourceBuilder<T> WithCreateNewMigrationCommand()
        {


            return builder.WithEfCoreCommands<T>(
                "add-ef-migration",
                "Add EF Migration",
                 _ =>
                {
                    var migrationName = "CHANGE_ME";

                    return Task.FromResult($"migrations add {migrationName} " +
                                           $"--project {MigrationsProjectPath} " +
                                           $"--startup-project {MigrationsProjectPath} " +
                                           "--output-dir Migrations");
                }
                );
        }

        internal IResourceBuilder<T> WithEfCoreCommands(string name, string displayName, Func<ExecuteCommandContext, Task<string>> getArgsAsync)
        {
            return builder.WithCommand(
                name: name,
                displayName: displayName,
                executeCommand: async context =>
                {
                    var connectionString = await builder.Resource.ConnectionStringExpression.GetValueAsync(context.CancellationToken);


                    var efArgs =$"ef {await getArgsAsync(context)}";

                    var startInfo = new ProcessStartInfo("dotnet", efArgs)
                    {
                        UseShellExecute = false,
                        WorkingDirectory = GetRepoRoot(),
                    };

                    startInfo.Environment["ConnectionStrings__postgres"] = connectionString;

                    try
                    {
                        using var process = Process.Start(startInfo);
                        process?.WaitForExit();

                        if (process != null && process.ExitCode != 0)
                        {
                            return new ExecuteCommandResult()
                            {
                                Success = false,
                                ErrorMessage = $"Command exited with code {process.ExitCode}"
                            };
                        }

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
                });;
        }
    }

    static string GetRepoRoot()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "LibraryManagement.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? Directory.GetCurrentDirectory();
    }
}

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
