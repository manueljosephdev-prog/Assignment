
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.Domain;
using Order.Infrastructure;
using System.Net.Http.Json;

[Route("api/orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IHttpClientFactory _http;
    private readonly IConfiguration _config;

    public OrdersController(AppDbContext db, IHttpClientFactory http, IConfiguration config)
    {
        _db = db;
        _http = http;
        _config = config;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Order order)
    {
        var client = _http.CreateClient();
        var baseUrl = _config["ProductServiceUrl"];

        foreach (var item in order.Items)
        {
            var p = await client.GetFromJsonAsync<ProductDto>($"{baseUrl}/{item.ProductId}");

            if (p == null || p.StockQty < item.Qty)
                return BadRequest($"Insufficient stock for product {item.ProductId}");
        }

        order.OrderId = Guid.NewGuid();
        order.OrderDate = DateTime.UtcNow;

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = order.OrderId }, order);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _db.Orders.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var o = await _db.Orders.FindAsync(id);
        return o == null ? NotFound() : Ok(o);
    }

    record ProductDto(Guid ProductId, string Name, decimal Price, int StockQty, string? ImageUrl);
}
