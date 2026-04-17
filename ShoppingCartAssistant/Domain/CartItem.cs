namespace ShoppingCartAssistant.Domain;

public class CartItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal TotalPrice => UnitPrice * Quantity;
}