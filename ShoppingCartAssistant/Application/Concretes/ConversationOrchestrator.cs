using System.Text.Json;
using ShoppingCartAssistant.Application.Abstracts;
using ShoppingCartAssistant.Application.Models;

namespace ShoppingCartAssistant.Application.Concretes;

public class ConversationOrchestrator : IConversationOrchestrator
{
    private readonly IChatAdapter _chatAdapter;
    private readonly IToolExecutor _toolExecutor;
    private readonly List<ChatMessage> _chatMessages = [];

    public ConversationOrchestrator(IChatAdapter chatModel, IToolExecutor toolExecutor)
    {
        _chatAdapter = chatModel;
        _toolExecutor = toolExecutor;

        _chatMessages.Add(new ChatMessage(ChatRole.System,
            "You are a shopping cart assistant. Use tools when needed."));
    }

    public async Task<string> AskAsync(string userInput, CancellationToken cancellationToken = default)
    {
        _chatMessages.Add(new ChatMessage(ChatRole.User, userInput));

        while (true)
        {
            var request = new ChatRequest(_chatMessages, _toolExecutor.Tools);
            var response = await _chatAdapter.CompleteAsync(request, cancellationToken);

            _chatMessages.Add(new ChatMessage(
                ChatRole.Assistant,
                response.Text ?? string.Empty,
                response.ToolCalls));

            if (!response.HasToolCalls)
            {
                return response.Text ?? string.Empty;
            }

            foreach (var toolCall in response.ToolCalls)
            {
                using var doc = JsonDocument.Parse(toolCall.ArgumentsJson);
                var result = _toolExecutor.Execute(toolCall.Name, doc.RootElement);

                _chatMessages.Add(new ChatMessage(
                    ChatRole.Tool,
                    Content: result,
                    ToolCallId: toolCall.Id,
                    Name: toolCall.Name));
            }
        }
    }
}