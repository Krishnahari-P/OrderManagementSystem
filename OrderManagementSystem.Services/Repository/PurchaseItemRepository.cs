using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services.Repository
{
    public class PurchaseItemRepository:IPurchaseItemRepository
    {
        private readonly AppDbContext _context;

        public PurchaseItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddPurchaseItemAsync(PurchaseItem purchaseItem)
        {
            _context.PurchaseItemSet.Add(purchaseItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePurchaseItemAsync(int id)
        {
            var purchaseItem = await _context.PurchaseItemSet.FindAsync(id);
            if (purchaseItem != null)
            {
                _context.PurchaseItemSet.Remove(purchaseItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<PurchaseItem>> GetAllPurchaseItemAsync(int purchaseItemId, int purchaseId, int productId)
        {
            var query = _context.PurchaseItemSet.Include(x => x.ProductSet).AsQueryable();
            if (purchaseItemId > 0)
            {
                query = query.Where(x => x.PurchaseItemId == purchaseItemId);
            }
            if (purchaseId > 0)
            {
                query = query.Where(x => x.PurchaseId == purchaseId);
            }
            if (productId > 0)
            {
                query = query.Where(x => x.ProductId == productId);
            }
            return await query.ToListAsync();
        }

        public async Task<PurchaseItem> GetPurchaseItemByIdAsync(int id)
        {
            var purchaseItem = await _context.PurchaseItemSet.FindAsync(id);
            return purchaseItem ?? throw new NotImplementedException("Purchase Item Not Found");
        }

        public async Task UpdatePurchaseItemAsync(PurchaseItem purchaseItem)
        {
            _context.PurchaseItemSet.Update(purchaseItem);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveItemAsync(int purchaseItemId)
        {
            var item = await _context.PurchaseItemSet
                .Include(pi => pi.PurchaseSet)
                .Include(pi => pi.ProductSet)
                .FirstOrDefaultAsync(pi => pi.PurchaseItemId == purchaseItemId);

            if (item == null)
            {
                return;
            }
            item.PurchaseSet.TotalAmount -= (item.Quantity * item.UnitCost);
            _context.PurchaseItemSet.Remove(item);
            await _context.SaveChangesAsync();
        }


        public async Task ReduceQuantityAsync(int purchaseItemId)
        {
            var item = await _context.PurchaseItemSet
                .Include(pi => pi.PurchaseSet)
                .Include(pi => pi.ProductSet)
                .FirstOrDefaultAsync(pi => pi.PurchaseItemId == purchaseItemId);

            if (item == null)
            {
                return;

            }
            var purchase = item.PurchaseSet;

            if (purchase == null)
            {
                return;
            }

            if (item.Quantity > 1)
            {
                item.Quantity--;
                purchase.TotalAmount -= item.UnitCost;
            }
            else
            {
                purchase.TotalAmount -= item.UnitCost;
                _context.PurchaseItemSet.Remove(item);
            }

            await _context.SaveChangesAsync();
        }
        public async Task<List<PurchaseItem>> GetPurchaseItemsByPurchaseIdAsync(int purchaseId)
        {
            return await _context.PurchaseItemSet
            .Include(pi => pi.ProductSet)
            .Include(pi => pi.PurchaseSet)
                .ThenInclude(p => p.SupplierSet)
            .Where(pi => pi.PurchaseId == purchaseId)
            .ToListAsync();
        }
    }
}
