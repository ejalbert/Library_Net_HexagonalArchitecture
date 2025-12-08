using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.TextGeneration;

namespace LibraryManagement.AI.SemanticKernel.SemanticKernel;

public interface ITokenAwareChatCompletionService : IChatCompletionService, ITextGenerationService
{
}
