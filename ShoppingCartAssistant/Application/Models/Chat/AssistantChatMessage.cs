using ShoppingCartAssistant.Application.Models.Tools;

namespace ShoppingCartAssistant.Application.Models.Chat;

public record AssistantChatMessage(
    string? Content,
    IReadOnlyList<ToolCall>? ToolCalls = null
) : IChatMessage
{
    public ChatRole ChatRole => ChatRole.Assistant;
}