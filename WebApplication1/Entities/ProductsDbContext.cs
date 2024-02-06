using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Entities
{
    public class ProductsDbContext : DbContext
    {

        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set;}
        public DbSet<Inventory> Inventories { get; set;}
        public DbSet<Price> Prices { get; set;}
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(a => a.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(a => a.FirstName)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(a => a.LastName)
                .IsRequired();
        }
    }
}
