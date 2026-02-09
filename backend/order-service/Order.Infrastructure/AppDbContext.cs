
using Microsoft.EntityFrameworkCore;
using Order.Domain;

namespace Order.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> o) : base(o) { }

    public DbSet<Order> Orders => Set<Order>();
}
