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
    }
}
