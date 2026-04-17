using ShoppingCartAssistant.Application.Models.Tools;

namespace ShoppingCartAssistant.Application.Models.Chat;

public record ToolCallChatResponse(IReadOnlyList<ToolCall> ToolCalls) : IChatResponse;