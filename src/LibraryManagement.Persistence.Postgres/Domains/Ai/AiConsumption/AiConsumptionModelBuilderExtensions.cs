using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Domains.Ai.AiConsumption;

internal static class AiConsumptionModelBuilderExtensions
{
    extension(ModelBuilder modelBuilder)
    {
        internal ModelBuilder ConfigureAiConsumptions()
        {
            modelBuilder.Entity<AiConsumptionEntity>(entity =>
            {
                entity.ToTable("AiConsumptions");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.InputTokens).IsRequired();
                entity.Property(e => e.OutputTokens).IsRequired();
                entity.Property(e => e.TotalTokens).IsRequired();
                entity.Property(e => e.ModelUsed).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            return modelBuilder;
        }
    }
}
