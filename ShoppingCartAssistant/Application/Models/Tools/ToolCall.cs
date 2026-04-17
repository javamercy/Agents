namespace ShoppingCartAssistant.Application.Models.Tools;

public record ToolCall(
    string Id,
    string Name,
    string ArgumentsJson
);