using OpenAI.Chat;
using ShoppingCartAssistant.Application.Models;

namespace ShoppingCartAssistant.Application.Extensions;

public static class ChatMessageExtensions
{
    public static List<OpenAI.Chat.ChatMessage> ToOpenAiChatMessages(
        this IEnumerable<Application.Models.ChatMessage> messages)
        => messages.Select(ToOpenAiChatMessage).ToList();

    private static OpenAI.Chat.ChatMessage ToOpenAiChatMessage(
        this Application.Models.ChatMessage message)
        => message.Role switch
        {
            ChatRole.System => new SystemChatMessage(message.Content),
            ChatRole.User => new UserChatMessage(message.Content),
            ChatRole.Assistant when message.ToolCalls is { Count: > 0 } => CreateAssistantToolCallMessage(message),
            ChatRole.Assistant => new AssistantChatMessage(message.Content),
            ChatRole.Tool => new ToolChatMessage(message.ToolCallId ?? string.Empty, message.Content),
            _ => throw new NotSupportedException($"Unsupported chat role: {message.Role}")
        };

    private static AssistantChatMessage CreateAssistantToolCallMessage(Application.Models.ChatMessage message)
    {
        var assistantMessage = new AssistantChatMessage(message.Content);
        foreach (var toolCall in message.ToolCalls ?? [])
        {
            assistantMessage.ToolCalls.Add(
                ChatToolCall.CreateFunctionToolCall(
                    toolCall.Id,
                    toolCall.Name,
                    BinaryData.FromString(toolCall.ArgumentsJson)));
        }

        return assistantMessage;
    }
}