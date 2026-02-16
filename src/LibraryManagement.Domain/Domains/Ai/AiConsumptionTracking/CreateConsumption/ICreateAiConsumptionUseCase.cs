namespace LibraryManagement.Domain.Domains.Ai.AiConsumptionTracking.CreateConsumption;

public interface ICreateAiConsumptionUseCase
{
    Task AddConsumptionAsync(CreateAiConsumptionCommand command, CancellationToken cancellationToken);
}
