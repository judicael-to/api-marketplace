// Data/InventoryContext.cs
using Microsoft.EntityFrameworkCore;
using SimpleApi.Models;

namespace SimpleApi.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships and constraints
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // Add seed data for testing
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and accessories" },
                new Category { Id = 2, Name = "Office Supplies", Description = "Items for office use" }
            );

            // Use hardcoded dates instead of DateTime.UtcNow
            var fixedDate = new DateTime(2023, 1, 1);

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Laptop",
                    SKU = "TECH-1001",
                    Price = 999.99m,
                    Cost = 750.00m,
                    Description = "High-performance laptop",
                    QuantityInStock = 15,
                    ReorderLevel = 5,
                    CategoryId = 1,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                },
                new Product
                {
                    Id = 2,
                    Name = "Notebook",
                    SKU = "OFF-2001",
                    Price = 4.99m,
                    Cost = 2.50m,
                    Description = "100-page lined notebook",
                    QuantityInStock = 100,
                    ReorderLevel = 20,
                    CategoryId = 2,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                }
            );
        }
    }
}