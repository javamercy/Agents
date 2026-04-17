using OpenAI.Chat;
using ShoppingCartAssistant.Application.Models;
using ShoppingCartAssistant.Application.Models.Tools;

namespace ShoppingCartAssistant.Application.Extensions;

public static class ToolDefinitionExtensions
{
    public static IReadOnlyList<OpenAI.Chat.ChatTool> ToOpenAiChatTools(this IEnumerable<ToolDefinition> tools)
        => tools.Select(ToOpenAiChatTool).ToList();

    private static OpenAI.Chat.ChatTool ToOpenAiChatTool(ToolDefinition tool)
        => ChatTool.CreateFunctionTool(
            functionName: tool.Name,
            functionDescription: tool.Description,
            functionParameters: BinaryData.FromString(tool.ParametersJson),
            functionSchemaIsStrict: tool.Strict);
}