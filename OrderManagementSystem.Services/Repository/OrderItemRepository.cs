using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services.Repository
{
    public class OrderItemRepository:IOrderItemRepository
    {
        private readonly AppDbContext _context;

        public OrderItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderItemAsync(OrderItem orderItem)
        {
            _context.OrderItemSet.Add(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderItemAsync(int id)
        {
            var orderItem = await _context.OrderItemSet.FindAsync(id);
            if (orderItem != null)
            {
                _context.OrderItemSet.Remove(orderItem);
                await _context.SaveChangesAsync();
            } 
        }

        public async Task<List<OrderItem>> GetAllOrderItemsAsync(int orderItemId, int orderId, int productId)
        {
            var query=from orderitem in _context.OrderItemSet select orderitem;
            if (orderItemId > 0)
            {
                query = query.Where(x => x.OrderItemId == orderItemId);
            }
            if (orderId > 0)
            {
                query = query.Where(x => x.OrderId == orderId);
            }
            if (productId > 0)
            {
                query = query.Where(x => x.ProductId == productId);
            }
            return await query.ToListAsync();
        }
        
        public async Task<OrderItem> GetOrderItemByIdAsync(int id)
        {
            var orderItem=await _context.OrderItemSet.FindAsync(id);
            return orderItem ?? throw new KeyNotFoundException("Order item not found");
        }

        public async Task UpdateOrderItemAsync(OrderItem orderItem)
        {
            _context.OrderItemSet.Update(orderItem);
            await _context.SaveChangesAsync();
        }
    }
}
