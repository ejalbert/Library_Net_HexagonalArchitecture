using OpenAI.Chat;

namespace LibraryManagement.AI.OpenAi.Domain.Common.Chat.Tools;

public interface IDomainChatTool
{
    string FunctionName { get; }

    ChatTool Tool { get; }

    Task<string> InvokeAsync(ChatToolCall toolCall);
}
