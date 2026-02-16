namespace LibraryManagement.Domain.Domains.Ai.AiConsumptionTracking.CreateConsumption;

public record CreateAiConsumptionCommand(long InputTokens, long OutputTokens, long TotalTokens, string ModelUsed);
