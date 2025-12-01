namespace LibraryManagement.Tests.Abstractions;

/// <summary>
/// Ensures Testcontainers talks to a Docker API version supported by older daemons.
/// Some environments expose a max API version of 1.43 and reject newer clients (1.44),
/// which causes fixtures to throw before starting containers.
/// </summary>
public static class DockerApiCompatibility
{
    private const string ApiVersionEnvVar = "DOCKER_API_VERSION";
    private const string CompatibleApiVersion = "1.43";

    private static bool _applied;

    public static void EnsureDockerApiVersion()
    {
        if (_applied)
        {
            return;
        }

        string? current = Environment.GetEnvironmentVariable(ApiVersionEnvVar);

        if (string.IsNullOrWhiteSpace(current))
        {
            Environment.SetEnvironmentVariable(ApiVersionEnvVar, CompatibleApiVersion);
        }

        _applied = true;
    }
}
