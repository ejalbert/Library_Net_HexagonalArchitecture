using Microsoft.AspNetCore.SignalR.Client;

namespace LibraryManagement.AI.SemanticKernel.LocalTools.Tools.ReadDirectory;

public class ReadDirectoryTool(HubConnection connection) : LocalToolBase(connection), IReadDirectoryTool
{
    public const string ToolName = "read_directory";

    public override Task RegisterAsync(CancellationToken cancellationToken = default)
    {
        Connection.On<string, string>(ToolName, async (corellationId, directoryPath) =>
        {
            IEnumerable<DirectoryEntry> entries = await ReadDirectoryAsync(directoryPath);
            await SendToolResponseAsync(corellationId, ToolName, entries);
        });

        return Task.CompletedTask;
    }

    public override Task UnregisterAsync(CancellationToken cancellationToken = default)
    {
        Connection.Remove(ToolName);

        return Task.CompletedTask;
    }

    private Task<IEnumerable<DirectoryEntry>> ReadDirectoryAsync(string? path)
    {
        ArgumentNullException.ThrowIfNull(path);

        if (!Directory.Exists(path)) throw new DirectoryNotFoundException($"The directory '{path}' does not exist.");

        IEnumerable<DirectoryEntry> entries = Directory.EnumerateFileSystemEntries(path)
            .Select(entry =>
                new DirectoryEntry(Path.GetFileName(entry), Directory.Exists(entry) ? "Directory" : "File"));

        return Task.FromResult(entries);
    }

    private record DirectoryEntry(string Name, string Type);
}
