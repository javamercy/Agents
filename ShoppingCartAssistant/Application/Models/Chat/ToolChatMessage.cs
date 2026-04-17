namespace ShoppingCartAssistant.Application.Models.Chat;

public record ToolChatMessage(
    string ToolCallId,
    string ToolCallName,
    string Content
) : IChatMessage
{
    public ChatRole ChatRole => ChatRole.Tool;
}