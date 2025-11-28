namespace LibraryManagement.Persistence.Postgres.ModuleConfiguration;

public class PersistencePostgresModuleOptions
{
    public required string ConnectionString { get; set; }
    public required string DatabaseName { get; set; }
}
