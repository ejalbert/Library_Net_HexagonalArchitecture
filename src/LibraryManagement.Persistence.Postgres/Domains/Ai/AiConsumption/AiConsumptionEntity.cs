using LibraryManagement.Persistence.Postgres.DbContexts.Multitenants;

namespace LibraryManagement.Persistence.Postgres.Domains.Ai.AiConsumption;

public class AiConsumptionEntity : MultitenantEntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long InputTokens { get; set; }
    public long OutputTokens { get; set; }
    public long TotalTokens { get; set; }
    public string ModelUsed { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
