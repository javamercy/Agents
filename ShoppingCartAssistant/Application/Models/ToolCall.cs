namespace ShoppingCartAssistant.Application.Models;

public record ToolCall(
    string Id,
    string Name,
    string ArgumentsJson
);