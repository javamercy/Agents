using ShoppingCartAssistant.Domain;

namespace ShoppingCartAssistant.Application.Concretes;

public class CartManager
{
    private readonly Cart _cart = new();

    public void Add(CartItem item)
    {
        item.Id = _cart.Items.Count > 0 ? _cart.Items.Max(i => i.Id) + 1 : 1;
        _cart.Items.Add(item);
    }

    public void Remove(int cartItemId)
    {
        var itemToRemove = _cart.Items.Find(c => c.Id == cartItemId);
        if (itemToRemove == null)
        {
            throw new Exception($"Item with id {cartItemId} not found");
        }

        _cart.Items.Remove(itemToRemove);
    }

    public void UpdateQuantity(int cartItemId, int quantity)
    {
        if (quantity < 0)
        {
            throw new Exception("Quantity cannot be negative");
        }

        var itemToUpdate = _cart.Items.Find(c => c.Id == cartItemId);
        if (itemToUpdate == null)
        {
            throw new Exception($"Item with id {cartItemId} not found");
        }

        if (quantity == 0)
        {
            _cart.Items.Remove(itemToUpdate);
        }
        else
        {
            itemToUpdate.Quantity = quantity;
        }
    }

    public decimal CalculateTotal()
    {
        return _cart.Items.Sum(c => c.TotalPrice);
    }
}