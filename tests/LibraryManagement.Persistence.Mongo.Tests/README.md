# LibraryManagement.Persistence.Mongo.Tests

Covers the MongoDB adapter end-to-end using xUnit and Testcontainers. The suite validates mapper logic and adapter behaviour for the `Books` aggregate.

## Notable Fixtures

- `MongoDbContainerFixture` spins up MongoDB 7 via Testcontainers, provides isolated databases per test, and drops them during teardown.
- `BookAdaptersIntegrationTests` verify create/get/search/delete flows against a real Mongo instance.
- Dedicated unit tests assert Mapperly-generated mappings plus adapter behaviours (create/search/get/delete).

## Commands

```bash
# Requires Docker running locally
dotnet test
```

Expect Docker to pull the `mongo:7.0` image the first time the suite runs.
