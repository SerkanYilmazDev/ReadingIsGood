using Microsoft.EntityFrameworkCore;
using Services.Orders.Data;

namespace Services.Customers.Data
{
    public class OrderDBContext : DbContext
    {
        public OrderDBContext(DbContextOptions<OrderDBContext> options) : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("order");
            modelBuilder.Entity<Order>().ToTable("orders");
            modelBuilder.Entity<Order>().HasMany(x => x.Items);
            modelBuilder.Entity<Order>().Property(s => s.Status).HasConversion<string>();
            modelBuilder.Entity<OrderItem>().ToTable("order_items");
        }
    }
}