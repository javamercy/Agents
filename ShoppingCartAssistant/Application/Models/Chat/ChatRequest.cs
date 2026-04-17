using ShoppingCartAssistant.Application.Models.Tools;

namespace ShoppingCartAssistant.Application.Models.Chat;

public record ChatRequest(
    IReadOnlyList<IChatMessage> Messages,
    IReadOnlyList<ToolDefinition> Tools);