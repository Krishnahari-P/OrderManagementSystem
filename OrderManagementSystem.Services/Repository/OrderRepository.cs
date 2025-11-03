using Microsoft.EntityFrameworkCore;
using NToastNotify;
using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddOrderAsync(Order order)
        {
            _context.OrderSet.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.OrderSet.FindAsync(id);
            if (order != null)
            {
                _context.OrderSet.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Order>> GetAllOrdersAsync(int orderId, DateTime orderDate, string status)
        {
            var query= from order in _context.OrderSet select order;
            if(orderId > 0)
            {
                query=query.Where(x => x.OrderId == orderId);
            }
            if (orderDate != default)
            {
                query=query.Where(x=>x.OrderDate == orderDate);
            }
            if (status != null)
            {
                query=query.Where(x=> x.Status.ToLower() == status.ToLower());
            }
            return await query.ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var order = await _context.OrderSet.Include(x => x.CustomerSet).ToListAsync();
            return order;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _context.OrderSet.FindAsync(id);
            return order ?? throw new KeyNotFoundException("Order not found");
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.OrderSet.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
