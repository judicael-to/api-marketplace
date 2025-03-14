// Models/Supplier.cs
namespace SimpleApi.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Navigation property
        public List<InventoryTransaction> Transactions { get; set; } = new List<InventoryTransaction>();
    }
}