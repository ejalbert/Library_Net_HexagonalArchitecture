namespace LibraryManagement.Domain.Domains.Ai.AiConsumptionTracking.CreateConsumption;

public interface ICreateAiConsumptionPort
{
    Task AddConsumptionAsync(long inputTokens, long outputTokens, long totalTokens, string modelUsed, CancellationToken cancellationToken);
}