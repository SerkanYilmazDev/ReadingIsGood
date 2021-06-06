using Microsoft.EntityFrameworkCore;

namespace Services.Customers.Data
{
    public class CustomerDBContext : DbContext
    {
        public CustomerDBContext(DbContextOptions<CustomerDBContext> options) : base(options)
        {
        }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("customer");
            modelBuilder.Entity<Basket>().ToTable("baskets");
            modelBuilder.Entity<Basket>().HasMany(x => x.Items);
            modelBuilder.Entity<BasketItem>().ToTable("basket_items");

            modelBuilder.Entity<Customer>().ToTable("customers");

            modelBuilder.Entity<Book>().ToTable("books_copy");
        }
    }
}