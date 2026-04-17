namespace ShoppingCartAssistant.Application.Models;

public record ChatRequest(
    IReadOnlyList<ChatMessage> Messages,
    IReadOnlyList<ToolDefinition> Tools);