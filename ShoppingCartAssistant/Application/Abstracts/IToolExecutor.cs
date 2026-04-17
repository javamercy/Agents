using System.Text.Json;
using ShoppingCartAssistant.Application.Models;
using ShoppingCartAssistant.Application.Models.Tools;

namespace ShoppingCartAssistant.Application.Abstracts;

public interface IToolExecutor
{
    IReadOnlyList<ToolDefinition> Tools { get; }

    string Execute(string toolName, JsonElement arguments);
}