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
            var query = _context.OrderItemSet.Include(x => x.ProductSet).AsQueryable();
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

        public async Task<List<OrderItem>> GetOrderItemByOrderIdAsync(int orderId)
        {
                return await _context.OrderItemSet
           .Include(oi => oi.ProductSet)
           .Include(oi => oi.OrderSet)
               .ThenInclude(o => o.CustomerSet)
           .Where(oi => oi.OrderId == orderId)
           .ToListAsync();
        }

        public async Task UpdateOrderItemAsync(OrderItem orderItem)
        {
            _context.OrderItemSet.Update(orderItem);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveItemAsync(int orderItemId)
        {
            var item = await _context.OrderItemSet
                .Include(oi => oi.OrderSet)
                .Include(oi => oi.ProductSet)
                .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);

            if (item == null) return;

            item.OrderSet.TotalAmount -= (item.Quantity * item.ProductSet.UnitPrice);
            _context.OrderItemSet.Remove(item);

            await _context.SaveChangesAsync();
        }
        public async Task ReduceQuantityAsync(int orderItemId)
        {
            var item = await _context.OrderItemSet
                .Include(oi => oi.OrderSet)
                .Include(oi => oi.ProductSet)
                .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);

            if (item == null) return;

            if (item.Quantity > 1)
            {
                item.Quantity--;
                item.OrderSet.TotalAmount -= item.ProductSet.UnitPrice;
            }
            else
            {
                item.OrderSet.TotalAmount -= item.ProductSet.UnitPrice;
                _context.OrderItemSet.Remove(item);
            }

            await _context.SaveChangesAsync();
        }
    }
}
