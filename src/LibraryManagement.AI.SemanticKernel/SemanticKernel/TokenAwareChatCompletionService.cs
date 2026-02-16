using System.Runtime.CompilerServices;

using LibraryManagement.Domain.Domains.Ai.AiConsumptionTracking.CreateConsumption;

using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Services;

using OpenAI.Chat;

using ChatMessageContent = Microsoft.SemanticKernel.ChatMessageContent;

namespace LibraryManagement.AI.SemanticKernel.SemanticKernel;

public class TokenAwareChatCompletionService(
    OpenAIChatCompletionService openAiChatCompletionService,
    ILogger<TokenAwareChatCompletionService> logger,
    ICreateAiConsumptionUseCase createAiConsumptionUseCase) : ITokenAwareChatCompletionService
{
    public IReadOnlyDictionary<string, object?> Attributes => openAiChatCompletionService.Attributes;

    public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null, CancellationToken cancellationToken = new())
    {
        IReadOnlyList<ChatMessageContent> chatContents =
            await openAiChatCompletionService.GetChatMessageContentsAsync(chatHistory, executionSettings, kernel,
                cancellationToken);

        long totalInputTokens = 0;
        long totalOutputTokens = 0;
        long totalTokens = 0;


        foreach (ChatMessageContent item in chatContents)
        {
            if (item.InnerContent is ChatCompletion chatCompletion)
            {
                totalInputTokens += chatCompletion.Usage.InputTokenCount;
                totalOutputTokens += chatCompletion.Usage.OutputTokenCount;
                totalTokens += chatCompletion.Usage.TotalTokenCount;
            }

            if (!string.IsNullOrWhiteSpace(item.Content)) logger.LogDebug(item.Content);
        }

        await createAiConsumptionUseCase.AddConsumptionAsync(
            new CreateAiConsumptionCommand(totalInputTokens, totalOutputTokens, totalTokens,
                openAiChatCompletionService.GetModelId() ?? "unknown"), cancellationToken);

        return chatContents;
    }

    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null, Kernel? kernel = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = new())
    {
        long totalInputTokens = 0;
        long totalOutputTokens = 0;
        long totalTokens = 0;

        try
        {
            await foreach (StreamingChatMessageContent item in openAiChatCompletionService
                               .GetStreamingChatMessageContentsAsync(chatHistory,
                                   executionSettings, kernel,
                                   cancellationToken))
            {
                if (item.InnerContent is ChatCompletion chatCompletion)
                {
                    totalInputTokens += chatCompletion.Usage.InputTokenCount;
                    totalOutputTokens += chatCompletion.Usage.OutputTokenCount;
                    totalTokens += chatCompletion.Usage.TotalTokenCount;
                }

                yield return item;
            }
        }
        finally
        {
            await createAiConsumptionUseCase.AddConsumptionAsync(
                new CreateAiConsumptionCommand(totalInputTokens, totalOutputTokens, totalTokens,
                    openAiChatCompletionService.GetModelId() ?? "unknown"), cancellationToken);
        }
    }

    public async Task<IReadOnlyList<TextContent>> GetTextContentsAsync(string prompt,
        PromptExecutionSettings? executionSettings = null, Kernel? kernel = null,
        CancellationToken cancellationToken = new())
    {
        IReadOnlyList<TextContent> chatContents =
            await openAiChatCompletionService.GetTextContentsAsync(prompt, executionSettings, kernel,
                cancellationToken);

        long totalInputTokens = 0;
        long totalOutputTokens = 0;
        long totalTokens = 0;


        foreach (TextContent item in chatContents)
        {
            if (item.InnerContent is ChatCompletion chatCompletion)
            {
                totalInputTokens += chatCompletion.Usage.InputTokenCount;
                totalOutputTokens += chatCompletion.Usage.OutputTokenCount;
                totalTokens += chatCompletion.Usage.TotalTokenCount;
            }

            if (!string.IsNullOrWhiteSpace(item.Text)) logger.LogDebug(item.Text);
        }

        await createAiConsumptionUseCase.AddConsumptionAsync(
            new CreateAiConsumptionCommand(totalInputTokens, totalOutputTokens, totalTokens,
                openAiChatCompletionService.GetModelId() ?? "unknown"), cancellationToken);

        return chatContents;
    }

    public async IAsyncEnumerable<StreamingTextContent> GetStreamingTextContentsAsync(string prompt,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null, [EnumeratorCancellation] CancellationToken cancellationToken = new())
    {
        long totalInputTokens = 0;
        long totalOutputTokens = 0;
        long totalTokens = 0;

        try
        {
            await foreach (StreamingTextContent item in openAiChatCompletionService.GetStreamingTextContentsAsync(
                               prompt, executionSettings, kernel,
                               cancellationToken))
            {
                if (item.InnerContent is ChatCompletion chatCompletion)
                {
                    totalInputTokens += chatCompletion.Usage.InputTokenCount;
                    totalOutputTokens += chatCompletion.Usage.OutputTokenCount;
                    totalTokens += chatCompletion.Usage.TotalTokenCount;
                }

                yield return item;
            }
        }
        finally
        {
            await createAiConsumptionUseCase.AddConsumptionAsync(
                new CreateAiConsumptionCommand(totalInputTokens, totalOutputTokens, totalTokens,
                    openAiChatCompletionService.GetModelId() ?? "unknown"), cancellationToken);
        }
    }
}
