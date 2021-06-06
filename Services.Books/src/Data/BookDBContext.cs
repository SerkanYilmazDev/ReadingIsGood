using Microsoft.EntityFrameworkCore;
using Services.Books.Data;

namespace Services.Customers.Data
{
    public class BookDBContext : DbContext
    {
        public BookDBContext(DbContextOptions<BookDBContext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("book");
            modelBuilder.Entity<Book>().ToTable("books");
        }
    }
}