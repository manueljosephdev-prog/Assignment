
namespace Product.Domain;

public class Product
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQty { get; set; }
    public string? ImageUrl { get; set; }
    public string? BlobName { get; set; }
}
