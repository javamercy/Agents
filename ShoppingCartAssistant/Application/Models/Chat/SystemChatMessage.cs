namespace ShoppingCartAssistant.Application.Models.Chat;

public record SystemChatMessage(
    string Content
) : IChatMessage
{
    public ChatRole ChatRole => ChatRole.System;
}