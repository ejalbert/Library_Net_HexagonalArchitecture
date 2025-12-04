namespace LibraryManagement.Domain.Domains.Ai.AiConsumptionTracking.CreateConsumption;

public class CreateAiConsumptionService(ICreateAiConsumptionPort createAiConsumptionPort) :ICreateAiConsumptionUseCase
{
    public Task AddConsumptionAsync(CreateAiConsumptionCommand command, CancellationToken cancellationToken = default)
    {
        return createAiConsumptionPort.AddConsumptionAsync(command.InputTokens, command.OutputTokens, command.TotalTokens, command.ModelUsed, cancellationToken);
    }
}
