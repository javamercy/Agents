namespace ShoppingCartAssistant.Application.Models.Chat;

public record UserChatMessage(
    string Content
) : IChatMessage
{
    public ChatRole ChatRole => ChatRole.User;
}