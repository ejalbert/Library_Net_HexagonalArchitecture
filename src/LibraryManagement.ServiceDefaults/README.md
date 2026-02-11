# LibraryManagement.ServiceDefaults

Shared Aspire service defaults for Library Management services. This project centralizes health checks, service
discovery, resilience, and OpenTelemetry configuration so every host can opt in consistently.

## Responsibilities

- Adds default OpenTelemetry logging, metrics, and tracing configuration.
- Registers service discovery and HTTP client resilience defaults.
- Defines health and liveness endpoints for development environments.

## Dependencies

- ASP.NET Core shared framework via `Microsoft.AspNetCore.App`.
- OpenTelemetry exporters and instrumentation packages.
- `Microsoft.Extensions.Http.Resilience` and `Microsoft.Extensions.ServiceDiscovery`.
- Shared architecture guidance: `../../docs/architecture.md`.

## Directory Layout

```
LibraryManagement.ServiceDefaults/
  Extensions.cs
```

## Commands

```bash
# Build the shared defaults library
dotnet build
```

## Tests

- No dedicated test project yet. Add `LibraryManagement.ServiceDefaults.Tests` if behavior expands beyond the default
  Aspire template guidance.

## Integration Points

- Inbound: `AddServiceDefaults()` and `MapDefaultEndpoints()` are called by service hosts.
- Outbound: OpenTelemetry exporters (OTLP or Azure Monitor when configured).

## Environment & Configuration

- `OTEL_EXPORTER_OTLP_ENDPOINT` enables OTLP exporting.
- `APPLICATIONINSIGHTS_CONNECTION_STRING` can enable Azure Monitor exporting (currently commented out).

## Related Documentation

- `../../README.md`
- `../../docs/architecture.md`

## Maintenance Notes

- TODO(agent): Add tests for health endpoint registration and OpenTelemetry toggles if customization is introduced.
