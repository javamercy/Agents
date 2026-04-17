using System.Text.Json;
using ShoppingCartAssistant.Application.Abstracts;
using ShoppingCartAssistant.Application.Models;
using ShoppingCartAssistant.Application.Models.Chat;

namespace ShoppingCartAssistant.Application.Concretes;

public class ConversationOrchestrator : IConversationOrchestrator
{
    private readonly IChatAdapter _chatAdapter;
    private readonly IToolExecutor _toolExecutor;
    private readonly List<IChatMessage> _chatMessages = [];

    public ConversationOrchestrator(IChatAdapter chatModel, IToolExecutor toolExecutor)
    {
        _chatAdapter = chatModel;
        _toolExecutor = toolExecutor;

        _chatMessages.Add(new SystemChatMessage("You are a shopping cart assistant. Use tools when needed."));
    }

    public async Task<string> AskAsync(string userInput, CancellationToken cancellationToken = default)
    {
        _chatMessages.Add(new UserChatMessage(userInput));

        while (true)
        {
            var request = new ChatRequest(_chatMessages, _toolExecutor.Tools);
            var response = await _chatAdapter.CompleteAsync(request, cancellationToken);

            switch (response)
            {
                case TextChatResponse textResponse:
                    _chatMessages.Add(new AssistantChatMessage(textResponse.Text));
                    return textResponse.Text;

                case ToolCallChatResponse toolResponse:
                    _chatMessages.Add(new AssistantChatMessage(null, toolResponse.ToolCalls));

                    foreach (var toolCall in toolResponse.ToolCalls)
                    {
                        using var doc = JsonDocument.Parse(toolCall.ArgumentsJson);
                        var result = _toolExecutor.Execute(toolCall.Name, doc.RootElement);

                        _chatMessages.Add(new ToolChatMessage(
                            ToolCallId: toolCall.Id,
                            ToolCallName: toolCall.Name,
                            Content: result));
                    }

                    break;

                default:
                    throw new NotSupportedException($"Unsupported chat response type: {response.GetType().Name}");
            }
        }
    }
}