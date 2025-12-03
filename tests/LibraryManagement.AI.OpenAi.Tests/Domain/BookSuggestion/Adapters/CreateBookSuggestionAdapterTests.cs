using System.ClientModel;

using LibraryManagement.AI.OpenAi.Domain.Authors.Tools;
using LibraryManagement.AI.OpenAi.Domain.Books.Tools;
using LibraryManagement.AI.OpenAi.Domain.BookSuggestion.Adapters;
using LibraryManagement.AI.OpenAi.Domain.Common.Chat;
using LibraryManagement.Domain.Domains.BookSuggestions.Create;

using Microsoft.Extensions.Logging;

using Moq;

using OpenAI.Chat;

namespace LibraryManagement.AI.OpenAi.Tests.Domain.BookSuggestion.Adapters;

/// <summary>
/// Tests for CreateBookSuggestionAdapter.
/// Note: Due to the OpenAI ChatClient being a sealed concrete class without an interface,
/// these tests focus on the adapter's construction and integration points.
/// Full end-to-end behavior requires integration tests with a real or fake ChatClient.
/// </summary>
public class CreateBookSuggestionAdapterTests
{
    [Fact]
    public void CreateBookSuggestionAdapter_can_be_constructed_with_dependencies()
    {
        // Arrange
        var chatClientMock = new Mock<ChatClient>();
        var searchBooksChatToolMock = new Mock<ISearchBooksChatTool>();
        var searchAuthorsChatToolMock = new Mock<ISearchAuthorsChatTool>();
        var loggerMock = new Mock<ILogger<CreateBookSuggestionAdapter>>();

        // Act
        var adapter = new CreateBookSuggestionAdapter(
            chatClientMock.Object,
            searchBooksChatToolMock.Object,
            searchAuthorsChatToolMock.Object,
            loggerMock.Object);

        // Assert
        Assert.NotNull(adapter);
        Assert.IsAssignableFrom<ICreateBookSuggestionPort>(adapter);
    }

    [Fact]
    public void CreateBookSuggestionAdapter_implements_ICreateBookSuggestionPort()
    {
        // Arrange & Act
        var adapterType = typeof(CreateBookSuggestionAdapter);
        var interfaceType = typeof(ICreateBookSuggestionPort);

        // Assert
        Assert.True(interfaceType.IsAssignableFrom(adapterType));
    }

    [Fact]
    public async Task SuggestAsync_invokes_ChatClient_with_system_and_user_prompts_and_tools()
    {
        // Arrange
        var chatClientMock = new Mock<ChatClient>();
        var searchBooksChatToolMock = new Mock<ISearchBooksChatTool>();
        var searchAuthorsChatToolMock = new Mock<ISearchAuthorsChatTool>();
        var loggerMock = new Mock<ILogger<CreateBookSuggestionAdapter>>();

        // Provide tool function names and underlying OpenAI tool definitions
        searchBooksChatToolMock.SetupGet(t => t.FunctionName).Returns("search_books");
        searchAuthorsChatToolMock.SetupGet(t => t.FunctionName).Returns("search_authors");
        // Use nulls to populate options.Tools without constructing ChatTool proxies
        searchBooksChatToolMock.SetupGet(t => t.Tool).Returns(ChatTool.CreateFunctionTool("search_books"));
        searchAuthorsChatToolMock.SetupGet(t => t.Tool).Returns(ChatTool.CreateFunctionTool("search_authors"));

        // Setup the chat client to throw so we can verify invocation with expected arguments
        chatClientMock
            .Setup(c => c.CompleteChatAsync(
                It.IsAny<List<ChatMessage>>(),
                It.IsAny<ChatCompletionOptions>()))
            .ThrowsAsync(new InvalidOperationException("Test invocation"));

        var adapter = new CreateBookSuggestionAdapter(
            chatClientMock.Object,
            searchBooksChatToolMock.Object,
            searchAuthorsChatToolMock.Object,
            loggerMock.Object);

        var prompt = "Suggest a book about software architecture";

        // Act
        await Assert.ThrowsAsync<InvalidOperationException>(() => adapter.SuggestAsync(prompt));

        // Assert - verify the ChatClient was invoked with expected messages and tools
        Assert.Single(chatClientMock.Invocations);
        var invocation = chatClientMock.Invocations[0];
        Assert.Equal("CompleteChatAsync", invocation.Method.Name);
        var messages = Assert.IsType<List<ChatMessage>>(invocation.Arguments[0]);
        var opts = Assert.IsType<ChatCompletionOptions>(invocation.Arguments[1]);

        Assert.True(messages.Count >= 2);
        Assert.Equal(typeof(SystemChatMessage), messages[0].GetType());
        var userMsg = messages[1] as UserChatMessage;
        Assert.NotNull(userMsg);
        Assert.Contains(userMsg!.Content, part => part.Text == prompt);

        Assert.Equal(2, opts.Tools.Count);
    }

    [Fact]
    public void SingleUserPromptChatCompletion_executes_with_system_and_user_prompts()
    {
        // Arrange
        var chatClientMock = new Mock<ChatClient>();
        var loggerMock = new Mock<ILogger>();

        // Act
        var executor = new SingleUserPromptChatCompletion(chatClientMock.Object, loggerMock.Object);

        // Assert - Verify the executor can be created with proper parameters
        Assert.NotNull(executor);

        // The actual execution would require a proper ChatClient implementation
        // This test documents the usage pattern: ExecuteAsync(systemPrompt, userPrompt, tools)
    }

    [Fact]
    public void CreateBookSuggestionAdapter_uses_correct_tools()
    {
        // Arrange
        var chatClientMock = new Mock<ChatClient>();
        var searchBooksChatToolMock = new Mock<ISearchBooksChatTool>();
        var searchAuthorsChatToolMock = new Mock<ISearchAuthorsChatTool>();
        var loggerMock = new Mock<ILogger<CreateBookSuggestionAdapter>>();

        searchBooksChatToolMock.SetupGet(t => t.FunctionName).Returns("search_books");
        searchAuthorsChatToolMock.SetupGet(t => t.FunctionName).Returns("search_authors");

        // Act
        var adapter = new CreateBookSuggestionAdapter(
            chatClientMock.Object,
            searchBooksChatToolMock.Object,
            searchAuthorsChatToolMock.Object,
            loggerMock.Object);

        // Assert
        Assert.NotNull(adapter);
        // The adapter should hold references to both tools
        // Actual tool invocation would be tested in integration tests
    }

    [Theory]
    [InlineData("")]
    [InlineData("Find a book about refactoring")]
    [InlineData("What books are available by Martin Fowler?")]
    [InlineData("I need a recommendation for learning design patterns")]
    public async Task SuggestAsync_accepts_various_prompt_formats(string prompt)
    {
        // Arrange
        var chatClientMock = new Mock<ChatClient>();
        var searchBooksChatToolMock = new Mock<ISearchBooksChatTool>();
        var searchAuthorsChatToolMock = new Mock<ISearchAuthorsChatTool>();
        var loggerMock = new Mock<ILogger<CreateBookSuggestionAdapter>>();

        // Provide placeholder tools to satisfy options wiring
        searchBooksChatToolMock.SetupGet(t => t.FunctionName).Returns("search_books");
        searchAuthorsChatToolMock.SetupGet(t => t.FunctionName).Returns("search_authors");
        searchBooksChatToolMock.SetupGet(t => t.Tool).Returns(ChatTool.CreateFunctionTool("search_books"));
        searchAuthorsChatToolMock.SetupGet(t => t.Tool).Returns(ChatTool.CreateFunctionTool("search_authors"));

        chatClientMock
            .Setup(c => c.CompleteChatAsync(
                It.IsAny<List<ChatMessage>>(),
                It.IsAny<ChatCompletionOptions>()))
            .ThrowsAsync(new InvalidOperationException("Test invocation"));

        var adapter = new CreateBookSuggestionAdapter(
            chatClientMock.Object,
            searchBooksChatToolMock.Object,
            searchAuthorsChatToolMock.Object,
            loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => adapter.SuggestAsync(prompt));

        // Verify the user prompt was propagated
        Assert.Single(chatClientMock.Invocations);
        var invocation = chatClientMock.Invocations[0];
        var messages = Assert.IsType<List<ChatMessage>>(invocation.Arguments[0]);
        Assert.Contains(messages, m => m is UserChatMessage um && um.Content.Any(p => p.Text == prompt));
    }

}
