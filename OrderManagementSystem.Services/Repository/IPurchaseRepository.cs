using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services.Repository
{
    public interface IPurchaseRepository
    {
        public Task<List<Purchase>> GetAllPurchaseAsync(int purchaseId, int supplierId,DateTime purchaseDate);
        public Task<Purchase> GetPurchaseByIdAsync(int id);
        public Task AddPurchaseAsync(Purchase purchase);
        public Task UpdatePurchaseAsync(Purchase purchase);
        public Task DeletePurchaseAsync(int id);
    }
}
