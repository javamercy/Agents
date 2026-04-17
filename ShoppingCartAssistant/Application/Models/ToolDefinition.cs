namespace ShoppingCartAssistant.Application.Models;

public record ToolDefinition(
    string Name,
    string Description,
    string ParametersJson,
    bool Strict = true);