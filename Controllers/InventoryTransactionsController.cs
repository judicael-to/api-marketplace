// Controllers/InventoryTransactionsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleApi.Data;
using SimpleApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryTransactionsController : ControllerBase
    {
        private readonly InventoryContext _context;

        public InventoryTransactionsController(InventoryContext context)
        {
            _context = context;
        }

        // GET: api/InventoryTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryTransaction>>> GetInventoryTransactions()
        {
            return await _context.InventoryTransactions
                .Include(t => t.Product)
                .Include(t => t.Supplier)
                .ToListAsync();
        }

        // GET: api/InventoryTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryTransaction>> GetInventoryTransaction(int id)
        {
            var transaction = await _context.InventoryTransactions
                .Include(t => t.Product)
                .Include(t => t.Supplier)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // POST: api/InventoryTransactions
        [HttpPost]
        public async Task<ActionResult<InventoryTransaction>> CreateInventoryTransaction(InventoryTransaction transaction)
        {
            // Start a transaction to ensure both operations complete or fail together
            using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Add the transaction record
                _context.InventoryTransactions.Add(transaction);

                // Update product inventory based on transaction type
                var product = await _context.Products.FindAsync(transaction.ProductId);
                if (product == null)
                {
                    return NotFound($"Product with ID {transaction.ProductId} not found");
                }

                // Adjust inventory based on transaction type
                switch (transaction.Type)
                {
                    case TransactionType.Purchase:
                        product.QuantityInStock += transaction.Quantity;
                        break;
                    case TransactionType.Sale:
                        if (product.QuantityInStock < transaction.Quantity)
                        {
                            return BadRequest($"Not enough inventory for product {product.Name}. Available: {product.QuantityInStock}");
                        }
                        product.QuantityInStock -= transaction.Quantity;
                        break;
                    case TransactionType.Adjustment:
                        // For direct adjustment, the quantity is the new value, not a delta
                        product.QuantityInStock = transaction.Quantity;
                        break;
                    case TransactionType.Return:
                        product.QuantityInStock += transaction.Quantity;
                        break;
                }

                product.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();

                return CreatedAtAction(nameof(GetInventoryTransaction), new { id = transaction.Id }, transaction);
            }
            catch (Exception)
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }

        // GET: api/InventoryTransactions/product/5
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<InventoryTransaction>>> GetTransactionsByProduct(int productId)
        {
            return await _context.InventoryTransactions
                .Where(t => t.ProductId == productId)
                .Include(t => t.Supplier)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }
    }
}