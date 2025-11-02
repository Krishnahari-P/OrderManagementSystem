using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services.Repository
{
    public interface IOrderRepository
    {
        public Task<List<Order>> GetAllOrdersAsync(int orderId, DateTime OrderDate, String status);
        public Task<Order> GetOrderByIdAsync(int id);
        public Task AddOrderAsync(Order order);
        public Task UpdateOrderAsync(Order order);
        public Task DeleteOrderAsync(int id);
    }
}
