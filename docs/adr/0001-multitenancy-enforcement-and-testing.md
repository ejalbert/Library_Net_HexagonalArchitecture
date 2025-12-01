# Multitenancy Enforcement and Testing in Postgres Persistence

- Status: accepted
- Deciders: maintainers, contributors
- Date: 2025-12-01

## Context

The Library Management System supports multitenancy, requiring strict isolation of data between tenants. The Postgres persistence layer uses an EF Core interceptor (`MultitenantSaveChangesInterceptor`) to enforce tenant boundaries by setting the `TenantId` property on all entities at save time, based on the current user's tenant ID. This is obtained via the `IGetCurrentUserTenantIdUseCase` port.

Testing this behaviour requires a pattern that works around the interceptor, which always overrides the `TenantId` property. Standard tests that set `TenantId` directly are ineffective, as the interceptor will replace it.

## Decision

- All persistence tests must use separate `DbContext` instances, each configured with a different mock of `IGetCurrentUserTenantIdUseCase`, to insert entities for different tenants.
- To validate tenant isolation, tests should:
  - Insert entities for multiple tenants using different contexts/mocks.
  - Query/update/delete entities using a context for a specific tenant and assert that only that tenant's data is accessible or modifiable.
  - Attempt to insert entities with a mismatched `TenantId` and assert that the interceptor overrides it to the current tenant.
- This pattern ensures the interceptor is exercised and tenant boundaries are strictly enforced.

## Consequences

- Multitenancy isolation is robustly validated in integration tests.
- Contributors must follow this pattern for all new persistence tests involving tenant boundaries.
- Documentation and contribution guidelines must be updated to reflect this pattern.
- The domain layer remains persistence-agnostic; tenant enforcement is handled in the infrastructure layer.

