namespace ShoppingCartAssistant.Application.Models;

public record ChatResponse(
    string? Text,
    IReadOnlyList<ToolCall> ToolCalls)
{
    public bool HasToolCalls => ToolCalls.Count > 0;
}