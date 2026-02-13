using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.AppHost.Extensions;

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
                async context =>
                {
                    var connectionString = await builder.Resource.ConnectionStringExpression.GetValueAsync(context.CancellationToken);

                    return "database update " +
                           $"--project {MigrationsProjectPath} " +
                           $"--startup-project {MigrationsProjectPath} "+
                           $"--connection {connectionString}";
                }
            );
        }

        internal IResourceBuilder<T> WithRevertAllMigrationsCommand()
        {
            return builder.WithEfCoreCommands<T>(
                "revert-all-database-migration",
                "Revert all Database Migration",
                async context =>
                {
                    var connectionString = await builder.Resource.ConnectionStringExpression.GetValueAsync(context.CancellationToken);



                    return "database update 0 " +
                           $"--project {MigrationsProjectPath} " +
                           $"--startup-project {MigrationsProjectPath} " +
                           $"--connection {connectionString}";
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
                async context =>
                {
                    var services = context.ServiceProvider;
#pragma warning disable ASPIREINTERACTION001
                    var interactionService =  services.GetRequiredService<IInteractionService>();
#pragma warning restore ASPIREINTERACTION001

                    var migrationNameResult = await interactionService.PromptInputAsync("New Migration Name", "Please enter a name for the new migration", "Migration Name", "Short clear name", new ()
                    {
                        ValidationCallback = ctx =>
                        {
                            var input = ctx.Inputs[0];
                            if(string.IsNullOrWhiteSpace(input.Value))
                            {
                                ctx.AddValidationError(input, "Migration name cannot be empty");
                            }

                            return Task.CompletedTask;
                        }
                    });

                    if (migrationNameResult is { Canceled: false, Data: not null })
                    {
                        var migrationName = migrationNameResult.Data?.Value ?? "CHANGE_ME";

                        return $"migrations add {migrationName} " +
                               $"--project {MigrationsProjectPath} " +
                               $"--startup-project {MigrationsProjectPath} " +
                               "--output-dir Migrations";
                    }
                    else
                    {
                        throw new TaskCanceledException();
                    }
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
