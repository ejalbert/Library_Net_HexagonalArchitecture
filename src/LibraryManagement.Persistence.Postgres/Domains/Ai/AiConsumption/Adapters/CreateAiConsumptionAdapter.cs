using LibraryManagement.Domain.Domains.Ai.AiConsumptionTracking.CreateConsumption;
using LibraryManagement.Persistence.Postgres.DbContexts;

namespace LibraryManagement.Persistence.Postgres.Domains.Ai.AiConsumption.Adapters;

public class CreateAiConsumptionAdapter(LibraryManagementDbContext context) : ICreateAiConsumptionPort
{
    public Task AddConsumptionAsync(long inputTokens, long outputTokens, long totalTokens, string modelUsed,
        CancellationToken cancellationToken)
    {
        context.TenantAiConsumptions.Add(new()
        {
            InputTokens =  inputTokens,
            OutputTokens = outputTokens,
            TotalTokens = totalTokens,
            ModelUsed = modelUsed
        });

        return context.SaveChangesAsync(cancellationToken);
    }
}
