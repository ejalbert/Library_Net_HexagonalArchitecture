namespace LibraryManagement.Persistence.Mongo.ModuleConfigurations;

public class PersistenceMongoModuleOptions
{
    public required string ConnectionString { get; set; }
    public required string DatabaseName { get; set; }
}