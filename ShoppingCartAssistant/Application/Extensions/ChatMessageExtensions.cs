using ShoppingCartAssistant.Application.Models.Chat;

namespace ShoppingCartAssistant.Application.Extensions;

public static class ChatMessageExtensions
{
    public static List<OpenAI.Chat.ChatMessage> ToOpenAiChatMessages(
        this IEnumerable<IChatMessage> messages)
        => messages.Select(ToOpenAiChatMessage).ToList();

    private static OpenAI.Chat.ChatMessage ToOpenAiChatMessage(
        this IChatMessage message)
        => message switch
        {
            SystemChatMessage m => new OpenAI.Chat.SystemChatMessage(m.Content),
            UserChatMessage m => new OpenAI.Chat.UserChatMessage(m.Content),
            ToolChatMessage m => new OpenAI.Chat.ToolChatMessage(m.ToolCallId, m.Content),
            AssistantChatMessage { ToolCalls.Count: > 0 } m => ToOpenAiAssistantChatMessage(m),
            AssistantChatMessage m =>
                new OpenAI.Chat.AssistantChatMessage(m.Content ?? string.Empty),
            _ => throw new NotSupportedException($"Unsupported chat message type: {message.GetType().Name}")
        };

    private static OpenAI.Chat.AssistantChatMessage ToOpenAiAssistantChatMessage(
        AssistantChatMessage message)
    {
        var assistant = new OpenAI.Chat.AssistantChatMessage(message.Content ?? string.Empty);

        foreach (var toolCall in message.ToolCalls ?? [])
        {
            assistant.ToolCalls.Add(
                OpenAI.Chat.ChatToolCall.CreateFunctionToolCall(
                    toolCall.Id,
                    toolCall.Name,
                    BinaryData.FromString(toolCall.ArgumentsJson)));
        }

        return assistant;
    }
}