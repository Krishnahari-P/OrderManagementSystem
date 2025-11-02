using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly AppDbContext _context;

        public PurchaseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddPurchaseAsync(Purchase purchase)
        {
            _context.PurchaseSet.Add(purchase);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePurchaseAsync(int id)
        {
            var purchase = await _context.PurchaseSet.FindAsync(id);
            if (purchase != null)
            {
                _context.PurchaseSet.Remove(purchase);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Purchase>> GetAllPurchaseAsync(int purchaseId, int supplierId, DateTime purchaseDate)
        {
            var query=from purchase in _context.PurchaseSet select purchase;
            if (purchaseId > 0)
            {
                query=query.Where(x=> x.PurchaseId == purchaseId);
            }
            if(supplierId > 0)
            {
                query=query.Where(x=> x.SupplierId == supplierId);
            }
            if (purchaseDate != default)
            {
                query=query.Where(x=>x.PurchaseDate == purchaseDate);
            }
            return await query.ToListAsync();
        }

        public async Task<Purchase> GetPurchaseByIdAsync(int id)
        {
            var purchase=await _context.PurchaseSet.FindAsync(id);
            return purchase?? throw new NotImplementedException("Purchase Not Found");
        }

        public async Task UpdatePurchaseAsync(Purchase purchase)
        {
            _context.PurchaseSet.Update(purchase);
            await _context.SaveChangesAsync();
        }
    }
}
