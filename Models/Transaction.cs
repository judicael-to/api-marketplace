// Models/InventoryTransaction.cs
namespace SimpleApi.Models
{
    public enum TransactionType
    {
        Purchase,
        Sale,
        Adjustment,
        Return
    }

    public class InventoryTransaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public TransactionType Type { get; set; }
        public decimal UnitPrice { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public int? SupplierId { get; set; }

        // Navigation properties
        public Product? Product { get; set; }
        public Supplier? Supplier { get; set; }
    }
}