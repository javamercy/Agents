using System.Globalization;
using System.Text.Json;
using OpenAI.Chat;

namespace MathToolCalling;

public sealed record ToolDefinition(string Name, string Description, string ParametersJson, bool Strict = true);

public class MathToolbox
{
    private const string AddToolName = "add";
    private const string MultiplyToolName = "multiply";
    private const string FactorialToolName = "factorial";

    public MathToolbox()
    {
        ToolDefinitions = new Dictionary<string, ToolDefinition>
        {
            [AddToolName] = new(
                AddToolName,
                "Returns the sum of two integers. Call this when the user asks to add numbers.",
                """
                {
                  "type": "object",
                  "properties": {
                    "a": {
                      "type": "number",
                      "description": "First number"
                    },
                    "b": {
                      "type": "number",
                      "description": "Second number"
                    }
                  },
                  "required": ["a", "b"],
                  "additionalProperties": false
                }
                """),
            [MultiplyToolName] = new(
                MultiplyToolName,
                "Returns the product of two integers. Call this when the user asks to multiply numbers.",
                """
                {
                  "type": "object",
                  "properties": {
                    "a": {
                      "type": "number",
                      "description": "First number"
                    },
                    "b": {
                      "type": "number",
                      "description": "Second number"
                    }
                  },
                  "required": ["a", "b"],
                  "additionalProperties": false
                }
                """),
            [FactorialToolName] = new(
                FactorialToolName,
                "Returns the factorial of a non-negative integer. Call this when the user asks for a factorial.",
                """
                {
                  "type": "object",
                  "properties": {
                    "n": {
                      "type": "integer",
                      "description": "Non-negative integer input"
                    }
                  },
                  "required": ["n"],
                  "additionalProperties": false
                }
                """)
        };
    }

    private IReadOnlyDictionary<string, ToolDefinition> ToolDefinitions { get; }

    public string Invoke(string toolName, JsonElement arguments)
    {
        return toolName switch
        {
            AddToolName => MathFunctions.Add(
                GetRequiredDouble(arguments, "a"),
                GetRequiredDouble(arguments, "b")).ToString(CultureInfo.InvariantCulture),

            MultiplyToolName => MathFunctions.Multiply(
                GetRequiredDouble(arguments, "a"),
                GetRequiredDouble(arguments, "b")).ToString(CultureInfo.InvariantCulture),

            FactorialToolName => InvokeFactorial(arguments),

            _ => throw new NotSupportedException($"Unknown tool '{toolName}'.")
        };
    }

    private string InvokeFactorial(JsonElement arguments)
    {
        var n = GetRequiredInt32(arguments, "n");
        return n < 0
            ? throw new ArgumentOutOfRangeException(nameof(n), "Argument 'n' must be non-negative.")
            : MathFunctions.Factorial(n).ToString(CultureInfo.InvariantCulture);
    }

    private double GetRequiredDouble(JsonElement arguments, string propertyName)
    {
        if (!arguments.TryGetProperty(propertyName, out var value))
            throw new ArgumentException($"Missing required argument '{propertyName}'.");

        if (value.ValueKind != JsonValueKind.Number || !value.TryGetDouble(out var doubleValue))
            throw new ArgumentException($"Argument '{propertyName}' must be a double.");

        return doubleValue;
    }

    private int GetRequiredInt32(JsonElement arguments, string propertyName)
    {
        if (!arguments.TryGetProperty(propertyName, out var value))
            throw new ArgumentException($"Missing required argument '{propertyName}'.");

        if (value.ValueKind != JsonValueKind.Number || !value.TryGetInt32(out var intValue))
            throw new ArgumentException($"Argument '{propertyName}' must be an integer.");

        return intValue;
    }

    public ToolDefinition? GetToolDefinition(string toolName)
    {
        return ToolDefinitions.GetValueOrDefault(toolName);
    }

    public IReadOnlyList<ChatTool> CreateSdkTools()
    {
        return ToolDefinitions.Values
            .Select(definition => ChatTool.CreateFunctionTool(
                definition.Name,
                definition.Description,
                BinaryData.FromString(definition.ParametersJson),
                definition.Strict
            )).ToArray();
    }
}