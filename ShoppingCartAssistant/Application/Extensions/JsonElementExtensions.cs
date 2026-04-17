using System.Text.Json;

namespace ShoppingCartAssistant.Application.Extensions;

public static class JsonElementExtensions
{
    extension(JsonElement root)
    {
        public int ParseInt(string name)
        {
            var value = root.GetRequiredProperty(name);
            if (value.ValueKind != JsonValueKind.Number || !value.TryGetInt32(out var result))
            {
                throw new ArgumentException($"Property '{name}' must be an integer.");
            }

            return result;
        }

        public decimal ParseDecimal(string name)
        {
            var value = root.GetRequiredProperty(name);
            if (value.ValueKind != JsonValueKind.Number || !value.TryGetDecimal(out var result))
            {
                throw new ArgumentException($"Property '{name}' must be a number.");
            }

            return result;
        }

        public string ParseString(string name)
        {
            var value = root.GetRequiredProperty(name);
            return value.GetString() ?? throw new ArgumentException($"Property '{name}' cannot be null.");
        }

        private JsonElement GetRequiredProperty(string name)
        {
            return !root.TryGetProperty(name, out var value)
                ? throw new ArgumentException($"Property '{name}' is required.")
                : value;
        }
    }
}