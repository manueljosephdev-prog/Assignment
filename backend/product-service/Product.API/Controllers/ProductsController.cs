
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.Domain;
using Product.Infrastructure;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly BlobService _blob;

    public ProductsController(AppDbContext db, BlobService blob)
    {
        _db = db;
        _blob = blob;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _db.Products.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var p = await _db.Products.FindAsync(id);
        return p == null ? NotFound() : Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product p)
    {
        p.ProductId = Guid.NewGuid();
        _db.Products.Add(p);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = p.ProductId }, p);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Product p)
    {
        var e = await _db.Products.FindAsync(id);
        if (e == null) return NotFound();

        e.Name = p.Name;
        e.Price = p.Price;
        e.StockQty = p.StockQty;
        await _db.SaveChangesAsync();
        return Ok(e);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var e = await _db.Products.FindAsync(id);
        if (e == null) return NotFound();

        if (!string.IsNullOrEmpty(e.BlobName))
            await _blob.DeleteAsync(e.BlobName);

        _db.Products.Remove(e);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id}/image")]
    public async Task<IActionResult> UploadImage(Guid id, IFormFile file)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();

        if (!string.IsNullOrEmpty(product.BlobName))
            await _blob.DeleteAsync(product.BlobName);

        var (url, blobName) = await _blob.UploadAsync(file);

        product.ImageUrl = url;
        product.BlobName = blobName;
        await _db.SaveChangesAsync();

        return Ok(new { imageUrl = url });
    }
}
