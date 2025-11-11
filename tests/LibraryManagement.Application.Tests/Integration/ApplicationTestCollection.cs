using LibraryManagement.Application.Tests.Infrastructure;

namespace LibraryManagement.Application.Tests.Integration;

public static class ApplicationTestCollection
{
    public const string Name = "LibraryManagementApplication";
}

[CollectionDefinition(ApplicationTestCollection.Name, DisableParallelization = true)]
public class ApplicationTestCollectionDefinition : ICollectionFixture<ApplicationWebApplicationFactory>;
