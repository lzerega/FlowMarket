namespace Orders.Api.Models;

public class Order
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Order() { }

    public Order(int id, int productId, int quantity, decimal total)
    {
        Id = id;
        ProductId = productId;
        Quantity = quantity;
        Total = total;
    }
}
