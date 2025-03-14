// Controllers/ReportsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleApi.Data;
using SimpleApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly InventoryContext _context;

        public ReportsController(InventoryContext context)
        {
            _context = context;
        }

        // GET: api/Reports/low-stock
        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<Product>>> GetLowStockProducts()
        {
            var lowStockProducts = await _context.Products
                .Where(p => p.QuantityInStock <= p.ReorderLevel)
                .Include(p => p.Category)
                .OrderBy(p => p.QuantityInStock)
                .ToListAsync();

            return Ok(lowStockProducts);
        }

        // GET: api/Reports/inventory-value
        [HttpGet("inventory-value")]
        public async Task<ActionResult<object>> GetInventoryValue()
        {
            var products = await _context.Products.ToListAsync();

            var totalCost = products.Sum(p => p.Cost * p.QuantityInStock);
            var totalRetail = products.Sum(p => p.Price * p.QuantityInStock);

            var inventoryValue = new
            {
                TotalCostValue = totalCost,
                TotalRetailValue = totalRetail,
                PotentialProfit = totalRetail - totalCost,
                TotalItems = products.Sum(p => p.QuantityInStock),
                UniqueProducts = products.Count
            };

            return Ok(inventoryValue);
        }

        // GET: api/Reports/transactions-summary
        [HttpGet("transactions-summary")]
        public async Task<ActionResult<object>> GetTransactionsSummary([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            // Default to last 30 days if no dates provided
            var start = startDate ?? DateTime.UtcNow.AddDays(-30);
            var end = endDate ?? DateTime.UtcNow;

            var transactions = await _context.InventoryTransactions
                .Where(t => t.TransactionDate >= start && t.TransactionDate <= end)
                .ToListAsync();

            var summary = new
            {
                TotalTransactions = transactions.Count,
                PurchaseTransactions = transactions.Count(t => t.Type == TransactionType.Purchase),
                SaleTransactions = transactions.Count(t => t.Type == TransactionType.Sale),
                AdjustmentTransactions = transactions.Count(t => t.Type == TransactionType.Adjustment),
                ReturnTransactions = transactions.Count(t => t.Type == TransactionType.Return),
                PurchaseValue = transactions.Where(t => t.Type == TransactionType.Purchase).Sum(t => t.UnitPrice * t.Quantity),
                SaleValue = transactions.Where(t => t.Type == TransactionType.Sale).Sum(t => t.UnitPrice * t.Quantity),
                StartDate = start,
                EndDate = end
            };

            return Ok(summary);
        }

        // GET: api/Reports/dashboard
        [HttpGet("dashboard")]
        public async Task<ActionResult<object>> GetDashboardSummary()
        {
            // Get counts
            var totalProducts = await _context.Products.CountAsync();
            var totalCategories = await _context.Categories.CountAsync();
            var totalSuppliers = await _context.Suppliers.CountAsync();

            // Get low stock count
            var lowStockCount = await _context.Products
                .CountAsync(p => p.QuantityInStock <= p.ReorderLevel);

            // Get recent transactions
            var recentTransactions = await _context.InventoryTransactions
                .Include(t => t.Product)
                .OrderByDescending(t => t.TransactionDate)
                .Take(5)
                .ToListAsync();

            // Calculate inventory value
            var products = await _context.Products.ToListAsync();
            var totalInventoryValue = products.Sum(p => p.Cost * p.QuantityInStock);

            // Get top products by value
            var topProductsByValue = await _context.Products
                .OrderByDescending(p => p.QuantityInStock * p.Price)
                .Take(5)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.QuantityInStock,
                    ValueInStock = p.QuantityInStock * p.Price
                })
                .ToListAsync();

            var dashboard = new
            {
                TotalProducts = totalProducts,
                TotalCategories = totalCategories,
                TotalSuppliers = totalSuppliers,
                LowStockItemsCount = lowStockCount,
                TotalInventoryValue = totalInventoryValue,
                RecentTransactions = recentTransactions,
                TopProductsByValue = topProductsByValue
            };

            return Ok(dashboard);
        }
    }
}