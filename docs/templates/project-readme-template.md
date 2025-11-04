# {Project Name}

> Replace `{Project Name}` with the concrete project folder name (e.g., `LibraryManagement.Api`).

## Purpose

- Briefly describe the responsibilities of this project (catalogue service, circulation worker, patron portal, etc.).
- Identify whether it is a domain/application component, an adapter, or a test suite.

## Dependencies

- List runtime and build dependencies that are specific to this project.
- Reference shared tooling using relative paths back to the repository root (e.g., ``../../docs/architecture.md``).

## Directory Layout

```
{provide a tree view or key folders/files}
```

## Commands

Run all commands from this directory unless noted otherwise. Use relative paths that traverse back to the repository root before targeting other folders.

```bash
# Build
dotnet build

# Run tests (example for test projects)
dotnet test

# Invoke shared script examples
../../scripts/<script-name>.sh
```

Replace the commands above with the actual steps for this project and verify they execute successfully from this directory.

## Tests

- This project is paired with `{Project}.Tests` using xUnit (add bUnit for UI adapters) and Moq for doubles.
- Document how to execute the relevant test suites from this directory, including integration tests when the project connects to external services (e.g., catalogue database, notification provider) or acts as a system entry point.
- Ensure every class containing business logic—loan calculations, patron validation, search orchestration—has corresponding tests; DTOs or simple markers without logic can be excluded.

## Integration Points

- List inbound or outbound ports implemented/consumed by this project.
- Call out external systems, queues, or databases touched by the adapters (e.g., MARC importer, message broker for overdue notices).

## Environment & Configuration

- Document required environment variables or settings files.
- Note how configuration is supplied when running in local and distributed environments.
- Reference shared Docker Compose services (e.g., ``../../docker-compose.yml``) that provision development dependencies such as MongoDB or Redis and include commands to start/stop them when needed.

## Related Documentation

- Link to ADRs, architecture notes, or additional docs using relative paths (e.g., ``../../docs/architecture.md``).

## Maintenance Notes

- Capture TODOs, follow-up work, or verification steps that future contributors should know.
