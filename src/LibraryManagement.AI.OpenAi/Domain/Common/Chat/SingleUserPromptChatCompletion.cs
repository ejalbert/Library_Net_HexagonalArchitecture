using LibraryManagement.AI.OpenAi.Domain.Common.Chat.Tools;

using Microsoft.Extensions.Logging;

using OpenAI.Chat;

namespace LibraryManagement.AI.OpenAi.Domain.Common.Chat;

public class SingleUserPromptChatCompletion(ChatClient chatClient, ILogger logger)
{
    public async Task<string> ExecuteAsync(string initialContextPrompt, string userPrompt, params IDomainChatTool[] tools)
    {
        var toolMap = new Dictionary<string, IDomainChatTool>();
        var chatCompletionOptions = new ChatCompletionOptions();

        foreach (var tool in tools)
        {
            toolMap[tool.FunctionName] = tool;
            chatCompletionOptions.Tools.Add(tool.Tool);
        }

        List<ChatMessage> conversation =
        [
            new SystemChatMessage(initialContextPrompt),
            new UserChatMessage(userPrompt)
        ];

        bool requiresAction;

        long outputTokes = 0;
        long inputTokens = 0;
        long totalTokens = 0;

        try
        {
            do
            {
                requiresAction = false;

                ChatCompletion completion = await chatClient.CompleteChatAsync(conversation, chatCompletionOptions);

                if (completion.Content.Count > 0)
                {
                    foreach (ChatMessageContentPart chatMessageContentPart in completion.Content)
                    {
                        logger.LogInformation("[Assistant]: {ContentPart}", chatMessageContentPart.Text);
                    }
                }

                var usage = completion.Usage;

                inputTokens += usage.InputTokenCount;
                outputTokes += usage.OutputTokenCount;
                totalTokens += usage.TotalTokenCount;

                switch (completion.FinishReason)
                {
                    case ChatFinishReason.Stop:
                        conversation.Add(new AssistantChatMessage(completion));
                        break;
                    case ChatFinishReason.ToolCalls:
                        conversation.Add(new AssistantChatMessage(completion));

                        foreach (var toolCall in completion.ToolCalls)
                        {
                            if (toolMap.TryGetValue(toolCall.FunctionName, out var tool))
                            {
                                conversation.Add(new ToolChatMessage(toolCall.Id, await tool.InvokeAsync(toolCall)));
                            }
                            else
                            {
                                conversation.Add(new ToolChatMessage(toolCall.Id,
                                    $"Error: Tool {toolCall.FunctionName} not found."));
                            }
                        }

                        requiresAction = true;
                        break;
                    case ChatFinishReason.Length:
                        throw new NotImplementedException(
                            "Incomplete model output due to MaxTokens parameter or token limit exceeded.");

                    case ChatFinishReason.ContentFilter:
                        throw new NotImplementedException("Omitted content due to a content filter flag.");

                    case ChatFinishReason.FunctionCall:
                        throw new NotImplementedException("Deprecated in favor of tool calls.");

                    default:
                        throw new NotImplementedException(completion.FinishReason.ToString());
                }

            } while (requiresAction);


            return conversation.Last().Content[0]?.Text ?? "No content";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during chat completion.");
            throw;
        }
        finally
        {
            logger.LogInformation("Tokens used: Input={InputTokens}, Output={OutputTokes}, Total={TotalTokensComputed}, TotalReported={TotalTokensReported}", inputTokens, outputTokes, inputTokens + outputTokes, totalTokens);
        }
    }
}
