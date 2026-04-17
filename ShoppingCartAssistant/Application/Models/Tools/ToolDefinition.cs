namespace ShoppingCartAssistant.Application.Models.Tools;

public record ToolDefinition(
    string Name,
    string Description,
    string ParametersJson,
    bool Strict = true);