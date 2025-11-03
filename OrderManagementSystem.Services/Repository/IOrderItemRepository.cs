using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services.Repository
{
    public interface IOrderItemRepository
    {
        public Task<List<OrderItem>> GetAllOrderItemsAsync(int orderItemId,int orderId,int productId);
        public Task<OrderItem> GetOrderItemByIdAsync(int id);
        public Task<List<OrderItem>> GetOrderItemByOrderIdAsync(int id);
        public Task AddOrderItemAsync(OrderItem orderItem);
        public Task UpdateOrderItemAsync(OrderItem orderItem);
        public Task DeleteOrderItemAsync(int id);
        public Task ReduceQuantityAsync(int orderItemId);
        public Task RemoveItemAsync(int orderItemId);
    }
}
