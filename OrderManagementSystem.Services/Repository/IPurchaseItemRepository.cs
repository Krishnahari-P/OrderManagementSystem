using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services.Repository
{
    public interface IPurchaseItemRepository
    {
        public Task<List<PurchaseItem>> GetAllPurchaseItemAsync(int purchaseItemId, int purchaseId, int productId);
        public Task<PurchaseItem> GetPurchaseItemByIdAsync(int id);
        public Task AddPurchaseItemAsync(PurchaseItem purchaseItem);
        public Task UpdatePurchaseItemAsync(PurchaseItem purchaseItem);
        public Task DeletePurchaseItemAsync(int id);
    }
}
