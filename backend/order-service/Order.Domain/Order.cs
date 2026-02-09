
namespace Order.Domain;

public class Order
{
    public Guid OrderId { get; set; }
    public string CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = "Pending";
    public List<OrderItem> Items { get; set; }
}

public class OrderItem
{
    public Guid ProductId { get; set; }
    public int Qty { get; set; }
}
