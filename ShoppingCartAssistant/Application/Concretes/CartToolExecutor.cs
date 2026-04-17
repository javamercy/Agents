using System.Globalization;
using System.Text.Json;
using ShoppingCartAssistant.Application.Abstracts;
using ShoppingCartAssistant.Application.Extensions;
using ShoppingCartAssistant.Application.Models;
using ShoppingCartAssistant.Application.Models.Tools;
using ShoppingCartAssistant.Domain;

namespace ShoppingCartAssistant.Application.Concretes;

public class CartToolExecutor : IToolExecutor
{
    private const string AddItem = "add_item";
    private const string RemoveItem = "remove_item";
    private const string UpdateQuantity = "update_quantity";
    private const string CalculateTotal = "calculate_total";

    private readonly CartManager _cartManager;

    public IReadOnlyList<ToolDefinition> Tools { get; }

    public CartToolExecutor(CartManager cartManager)
    {
        _cartManager = cartManager;

        Tools =
        [
            new ToolDefinition(
                AddItem,
                "Add a product to the cart.",
                """
                {
                  "type": "object",
                  "properties": {
                    "name": { "type": "string", "description": "Product name." },
                    "price": { "type": "number", "description": "Unit price." },
                    "quantity": { "type": "integer", "description": "Quantity to add." }
                  },
                  "required": ["name", "price", "quantity"],
                  "additionalProperties": false
                }
                """
            ),
            new ToolDefinition(
                RemoveItem,
                "Remove a product from the cart by cart item id.",
                """
                {
                  "type": "object",
                  "properties": {
                    "id": { "type": "integer", "description": "Cart item id." }
                  },
                  "required": ["id"],
                  "additionalProperties": false
                }
                """
            ),
            new ToolDefinition(
                UpdateQuantity,
                "Update the quantity of a cart item.",
                """
                {
                  "type": "object",
                  "properties": {
                    "id": { "type": "integer", "description": "Cart item id." },
                    "quantity": { "type": "integer", "description": "New quantity." }
                  },
                  "required": ["id", "quantity"],
                  "additionalProperties": false
                }
                """
            ),
            new ToolDefinition(
                CalculateTotal,
                "Calculate the cart total.",
                """
                {
                  "type": "object",
                  "properties": {},
                  "required": [],
                  "additionalProperties": false
                }
                """
            )
        ];
    }

    public string Execute(string toolName, JsonElement arguments)
    {
        return toolName switch
        {
            AddItem => ExecuteAddItem(arguments),
            RemoveItem => ExecuteRemoveItem(arguments),
            UpdateQuantity => ExecuteUpdateQuantity(arguments),
            CalculateTotal => _cartManager.CalculateTotal().ToString(CultureInfo.InvariantCulture),
            _ => throw new NotSupportedException($"Unknown tool '{toolName}'.")
        };
    }

    private string ExecuteAddItem(JsonElement arguments)
    {
        var name = arguments.ParseString("name");
        var quantity = arguments.ParseInt("quantity");
        var unitPrice = arguments.ParseDecimal("price");

        var cartItem = new CartItem
        {
            Name = name,
            Quantity = quantity,
            UnitPrice = unitPrice
        };

        _cartManager.Add(cartItem);
        return $"Added product {name} with quantity {quantity} and unit price {unitPrice} to the cart.";
    }

    private string ExecuteRemoveItem(JsonElement arguments)
    {
        var id = arguments.ParseInt("id");
        _cartManager.Remove(id);
        return $"Removed product #{id} from the cart.";
    }

    private string ExecuteUpdateQuantity(JsonElement arguments)
    {
        var id = arguments.ParseInt("id");
        var quantity = arguments.ParseInt("quantity");
        _cartManager.UpdateQuantity(id, quantity);
        return $"Updated item #{id} to quantity {quantity}.";
    }
}